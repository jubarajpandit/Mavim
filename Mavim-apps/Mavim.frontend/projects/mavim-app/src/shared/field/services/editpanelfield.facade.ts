import { Injectable, OnDestroy } from '@angular/core';
import { FieldService } from './field.service';
import * as fieldActions from '../+state/actions/field.actions';
import { Store } from '@ngrx/store';
import { Field } from '../models/field.model';
import {
	selectFieldByID,
	selectFieldsByID
} from '../+state/selectors/field.selectors';
import { Subject } from 'rxjs';
import { takeUntil, take } from 'rxjs/operators';
import { IEditPanelFacade } from '../../../containers/edit/components/interfaces/iedit-panel.facade';

@Injectable()
export class EditPanelFacade implements IEditPanelFacade, OnDestroy {
	public constructor(
		private readonly fieldService: FieldService,
		private readonly store: Store
	) {}
	private readonly unsubscribe$ = new Subject();

	public updateFieldValues(newFieldValue: Field): void {
		this.store
			.select(
				selectFieldByID(
					`${newFieldValue.topicDCV}_${newFieldValue.fieldSetId}_${newFieldValue.fieldId}`
				)
			)
			.pipe(take(1), takeUntil(this.unsubscribe$))
			.subscribe((oldFieldValue: Field) => {
				this.store.dispatch(
					fieldActions.UpdateField({ payload: newFieldValue })
				);
				this.updateField(newFieldValue, oldFieldValue);
			});
	}

	public updateFieldsValues(fields: Field[]): void {
		const fieldIds = fields.map(
			(f) => `${f.topicDCV}_${f.fieldSetId}_${f.fieldId}`
		);
		this.store
			.select(selectFieldsByID(fieldIds))
			.pipe(take(1), takeUntil(this.unsubscribe$))
			.subscribe((oldFieldValue: Field[]) => {
				this.store.dispatch(
					fieldActions.UpdateFields({ payload: fields })
				);
				this.updateFields(fields, oldFieldValue);
			});
	}

	public ngOnDestroy(): void {
		this.unsubscribe$.next(true);
		this.unsubscribe$.complete();
	}

	private updateField(newFieldValue: Field, oldFieldValue: Field): void {
		this.fieldService
			.updateFieldValues(newFieldValue)
			.pipe(takeUntil(this.unsubscribe$))
			.subscribe(
				() => {
					this.store.dispatch(fieldActions.UpdateFieldSuccess());
				},
				() => {
					this.store.dispatch(
						fieldActions.UpdateFieldFail({ payload: oldFieldValue })
					);
				}
			);
	}

	private updateFields(
		newFieldsValues: Field[],
		oldFieldValue: Field[]
	): void {
		this.fieldService
			.updateFieldsValues(newFieldsValues)
			.pipe(takeUntil(this.unsubscribe$))
			.subscribe(
				(response) => {
					if (
						response &&
						response.failed &&
						response.failed.length > 0
					) {
						const failedFieldIds = Array.from(
							new Set(response.failed.map((s) => s.item.fieldId))
						);
						const failedOldFieldValues = failedFieldIds.map(
							(id) => {
								return {
									...oldFieldValue.find(
										(s) => s.fieldId === id
									)
								};
							}
						);
						this.store.dispatch(
							fieldActions.UpdateFieldsFail({
								payload: failedOldFieldValues
							})
						);
					} else {
						this.store.dispatch(fieldActions.UpdateFieldsSuccess());
					}
				},
				() => {
					this.store.dispatch(
						fieldActions.UpdateFieldsFail({
							payload: oldFieldValue
						})
					);
				}
			);
	}
}
