import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { Observable, combineLatest, Subject } from 'rxjs';
import { Store } from '@ngrx/store';
import { Field } from '../../../../shared/field/models/field.model';
import { FormValidationError } from '../../models/formvalidationerror.model';
import { getNotifications } from '../../../../shared/notification/+state';
import {
	selectTopicById,
	topicLoaded
} from '../../../../shared/topic/+state/selectors/topic.selector';
import {
	selectFieldByDcv,
	allFieldsLoaded
} from '../../../../shared/field/+state/selectors/field.selectors';
import {
	selectRelationsByDcv,
	allRelationsLoaded
} from '../../../../shared/relation/+state/selectors/relation.selectors';
import * as topicActions from '../../../../shared/topic/+state/actions/topic.actions';
import * as relationActions from '../../../../shared/relation/+state/actions/relation.actions';
import * as fieldActions from '../../../../shared/field/+state/actions/field.actions';
import { NotificationService } from '../../../../shared/notification/services/notification.service';
import { takeUntil, tap, delay, take } from 'rxjs/operators';
import { NotificationTypes } from '../../../../shared/notification/enums/notification-types.enum';
import { EditStatus } from '../../enums/edit-status.enum';
import { IEditPanelFacade } from '../interfaces/iedit-panel.facade';
import { EDITPANEL_FACADE } from '../tokens/edit-panel.token';
import { EditRelation } from '../../../../shared/relation/models/edit-relation.model';
import { Relation } from '../../../../shared/relation/models/relation.model';
import { EditField } from '../../../../shared/field/models/edit-field.model';
import { EditTopic } from '../../../../shared/topic/models/edit-topic.model';
import { ErrorService } from '../../../../shared/notification/services/error.service';
import { ActivatedRoute } from '@angular/router';
import { RegexUtils } from '../../../../shared/utils';
import { RoutingFacade } from '../../../../shared/router/service';
import { FeatureflagFacade } from '../../../../shared/featureflag/service/featureflag.facade';
import { relationFeatures } from '../../../../shared/relation/enums/relationfeatures';

@Component({
	selector: 'mav-edit-panel',
	templateUrl: './edit-panel.component.html',
	styleUrls: ['./edit-panel.component.scss']
})
export class EditPanelComponent implements OnInit, OnDestroy {
	public constructor(
		private readonly store: Store,
		private readonly notificationService: NotificationService,
		private readonly route: ActivatedRoute,
		@Inject(EDITPANEL_FACADE)
		private readonly editPanelFacade: IEditPanelFacade,
		private readonly errorService: ErrorService,
		private readonly editRouting: RoutingFacade,
		private readonly featureflagFacade: FeatureflagFacade
	) {
		this.topicLoaded$ = this.store.select(topicLoaded).pipe(delay(0));
		this.fieldsLoaded$ = this.store.select(allFieldsLoaded).pipe(delay(0));
		this.relationsLoaded$ = this.store
			.select(allRelationsLoaded)
			.pipe(delay(0));
		const dcvid = this.route.snapshot.params['dcvid'] as string;
		this.dcvId = dcvid;
	}

	public topicData: EditTopic = undefined;
	public fieldData: EditField[] = [];
	public relationData: EditRelation[] = [];

	public readonly topicLoaded$: Observable<boolean>;
	public readonly relationsLoaded$: Observable<boolean>;
	public readonly fieldsLoaded$: Observable<boolean>;

	public validationErrors: { [key: string]: FormValidationError } = {};

	// TODO: Move to translations
	public editButtonClass = 'save';
	public closeButtonClass = 'cancel';
	public saveButtonText = 'Save';
	public editButtonText = 'Edit';
	public closeButtonText = 'Close';
	public cancelButtonText = 'Cancel';

	public showConfirmClose = false;
	public showSaving = false;
	public showModal = false;
	public openChanges = false;
	public relationFeatureFlag: boolean;

	public readonly dcvId: string;

	private readonly destroySubject$ = new Subject();

	public ngOnInit(): void {
		this.initFeatureflags();
		if (RegexUtils.dcvID().test(this.dcvId)) {
			this.getTopicData();
			this.getFieldData();
			this.getRelationData();
		} else {
			this.errorService.handleClientError('DcvID invalid', 'routeParams');
		}
	}

	public ngOnDestroy(): void {
		this.destroySubject$.next();
		this.destroySubject$.complete();
	}

	public handleAddRelationEvent(): void {
		this.showModal = true;
	}

	public hideCreateRelationsModal(): void {
		this.showModal = false;
	}

	public handleResetRelationEvent(relation: EditRelation): void {
		relation.status = EditStatus.Unchanged;
	}

	public handleDeleteRelationEvent(relation: EditRelation): void {
		if (relation.status === EditStatus.Unchanged) {
			relation.status = EditStatus.Deleted;
		} else if (relation.status === EditStatus.Created) {
			this.deleteSoftSavedRelation(relation);
		}
	}

	public trySaveChanges(): void {
		if (Object.keys(this.validationErrors).length > 0) {
			this.showValidationErrors();
		} else {
			this.setOpenChanges();
			if (this.openChanges) {
				this.saveAll();
			} else {
				this.showConfirmClose = true;
			}
		}
	}

	public cancelEdited(): void {
		this.setOpenChanges();
		if (this.openChanges) {
			this.showConfirmClose = true;
		} else {
			this.closeEdited();
		}
	}

	public backToEdit(): void {
		this.showConfirmClose = false;
	}

	public closeEdited(): void {
		this.editRouting.backEdit();
	}

	public onValidationError(
		error: FormValidationError,
		section: string
	): void {
		this.validationErrors[section] = error;
	}

	public addRelation(relation: EditRelation): void {
		this.relationData.unshift(relation);
		if (this.showModal) {
			this.hideCreateRelationsModal();
		}
	}

	public handleFieldsChanged(data: EditField[], section: string): void {
		this.fieldData = data;

		this.clearValidationErrors(section);
	}

	public handleTopicChanged(updatedTopic: string, section: string): void {
		this.topicData = {
			...this.topicData,
			dcv: this.dcvId,
			name: updatedTopic,
			editStatus: EditStatus.Updated
		};
		this.clearValidationErrors(section);
	}

	private showValidationErrors(): void {
		const openErrors = Object.keys(this.validationErrors).join(', ');
		const grammer =
			Object.keys(this.validationErrors).length === 1
				? ['is', 'error']
				: ['are', 'errors'];
		const message = `There ${grammer[0]} ${
			Object.keys(this.validationErrors).length
		} open validation ${grammer[1]} at: ${openErrors}`;
		this.notificationService.sendNotification(
			NotificationTypes.Error,
			message
		);
	}

	private saveAll(): void {
		this.showSaving = true;

		this.saveTopic();
		this.saveFields();
		this.saveRelations();
		this.closeEditWhenSavingIsComplete();
	}

	private clearValidationErrors(section: string): void {
		delete this.validationErrors[section];
	}

	private getTopicData(): void {
		let fetchTopic = false;
		const topicSubscription = new Subject();
		this.store
			.select(selectTopicById(this.dcvId))
			.pipe(
				tap((topic) => {
					if (!topic && !fetchTopic) {
						fetchTopic = true;
						this.store.dispatch(
							topicActions.LoadTopicByDCV({ payload: this.dcvId })
						);
					}
				}),
				takeUntil(topicSubscription)
			)
			.subscribe((topic) => {
				if (topic) {
					this.topicData = {
						...topic,
						editStatus: EditStatus.Unchanged
					} as EditTopic;
					topicSubscription.next();
					topicSubscription.complete();
				}
			});
	}

	private getFieldData(): void {
		let fetchFields = false;
		const fieldsSubscription = new Subject();
		combineLatest([
			this.store.select(selectFieldByDcv(this.dcvId)).pipe(
				tap((fields) => {
					if ((!fields || fields.length === 0) && !fetchFields) {
						fetchFields = true;
						this.store.dispatch(
							fieldActions.LoadFields({ payload: this.dcvId })
						);
					}
				}),
				takeUntil(fieldsSubscription)
			),
			this.fieldsLoaded$
		]).subscribe(([fields, fieldsLoaded]) => {
			if (fieldsLoaded) {
				const mappedFields = fields?.map((field: Field): EditField => {
					const data = field.data ? field.data.map((i) => i) : [];
					return {
						...field,
						data,
						status: EditStatus.Unchanged
					};
				});
				this.fieldData = mappedFields || [];
				fieldsSubscription.next();
				fieldsSubscription.complete();
			}
		});
	}

	private getRelationData(): void {
		let fetchRelations = false;
		const relationsSubscription = new Subject();
		combineLatest([
			this.store.select(selectRelationsByDcv(this.dcvId)).pipe(
				tap((relation) => {
					if (
						(!relation || relation.length === 0) &&
						!fetchRelations
					) {
						fetchRelations = true;
						this.store.dispatch(
							relationActions.LoadRelations({
								payload: this.dcvId
							})
						);
					}
				}),
				takeUntil(relationsSubscription)
			),
			this.relationsLoaded$
		]).subscribe(([relations, relationsLoaded]) => {
			if (relationsLoaded) {
				const mappedRelations = relations?.map(
					(relation: Relation): EditRelation => ({
						...relation,
						status: EditStatus.Unchanged
					})
				);
				this.relationData = mappedRelations || [];
				relationsSubscription.next();
				relationsSubscription.complete();
			}
		});
	}

	private setOpenChanges(): void {
		const topicChanged: boolean =
			this.topicData &&
			this.topicData.editStatus !== EditStatus.Unchanged;
		const fieldsChanged: boolean =
			this.fieldData &&
			this.fieldData.filter(
				(field) => field.status !== EditStatus.Unchanged
			).length > 0;
		const relationsChanged: boolean =
			this.relationData &&
			this.relationData.filter(
				(relation) => relation.status !== EditStatus.Unchanged
			).length > 0;

		this.openChanges = topicChanged || fieldsChanged || relationsChanged;
	}

	private closeEditWhenSavingIsComplete(): void {
		const destroySubscription$: Subject<void> = new Subject();
		combineLatest([
			this.topicLoaded$,
			this.fieldsLoaded$,
			this.relationsLoaded$,
			this.store.select(getNotifications)
		])
			.pipe(takeUntil(destroySubscription$))
			.subscribe(
				([
					topicUpdated,
					fieldsUpdated,
					relationsupdated,
					notifications
				]) => {
					// wait until http calls are done
					if (topicUpdated && fieldsUpdated && relationsupdated) {
						const errors = notifications.filter(
							(notification) =>
								notification.type === NotificationTypes.Error
						);
						// destroy current subscription
						destroySubscription$.next();
						destroySubscription$.complete();

						// no errors during saving, close the panel
						if (errors.length === 0) {
							this.showSuccessNotification();
							this.editRouting.backEdit();
						} else {
							this.showSaving = false;
							this.showConfirmClose = false;
						}
					}
				}
			);
	}

	private showSuccessNotification(): void {
		this.notificationService.sendNotification(
			NotificationTypes.Success,
			`Changes saved!`
		);
	}

	private saveTopic(): void {
		if (
			this.topicData &&
			this.topicData.editStatus !== EditStatus.Unchanged
		) {
			this.store.dispatch(
				topicActions.UpdateTopicName({ payload: this.topicData })
			);
		}
	}

	private saveFields(): void {
		if (this.fieldData && this.fieldData.length > 0) {
			const editFields = this.fieldData.filter(
				(f) => f.status !== EditStatus.Unchanged
			);
			if (editFields.length === 1) {
				this.editPanelFacade.updateFieldValues(editFields[0]);
			} else if (editFields.length > 0) {
				this.editPanelFacade.updateFieldsValues(editFields);
			}
		}
	}

	private saveRelations(): void {
		if (this.relationData && this.relationData.length > 0) {
			this.relationData.forEach((relation) => {
				if (relation.status === EditStatus.Created) {
					this.store.dispatch(
						relationActions.CreateRelation({ payload: relation })
					);
				} else if (relation.status === EditStatus.Deleted) {
					this.store.dispatch(
						relationActions.DeleteRelation({ payload: relation })
					);
				}
			});
		}
	}

	private deleteSoftSavedRelation(relation: EditRelation): void {
		const index = this.relationData.indexOf(relation);
		if (index > -1) {
			this.relationData.splice(index, 1);
		}
	}

	private initFeatureflags(): void {
		this.featureflagFacade
			.getFeatureflag(relationFeatures.norelation)
			.pipe(take(1))
			.subscribe((featureEnabled) => {
				this.relationFeatureFlag = featureEnabled;
			});
	}
}
