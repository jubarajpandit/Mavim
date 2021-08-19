import { WopiActionUrls } from './../../../../shared/wopi/models/wopi-actionurls.model';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Topic } from '../../../../shared/topic/models/topic.model';
import { Store } from '@ngrx/store';
import {
	selectChildTopicsByDcv,
	selectTopicById
} from '../../../../shared/topic/+state/selectors/topic.selector';
import { selectFieldByDcv } from '../../../../shared/field/+state/selectors/field.selectors';
import { selectRelationsByDcv } from '../../../../shared/relation/+state/selectors/relation.selectors';
import { Field } from '../../../../shared/field/models/field.model';
import { Relation } from '../../../../shared/relation/models/relation.model';
import * as topicActions from '../../../../shared/topic/+state/actions/topic.actions';
import * as fieldActions from '../../../../shared/field/+state/actions/field.actions';
import * as relationActions from '../../../../shared/relation/+state/actions/relation.actions';
import { map, tap, delay, takeUntil, filter, take } from 'rxjs/operators';
import { ActivatedRoute, Params } from '@angular/router';
import { Observable } from 'rxjs';
import { Subject } from 'rxjs';
import { ErrorService } from '../../../../shared/notification/services/error.service';
import { RegexUtils } from '../../../../shared/utils';
import { RoutingFacade } from '../../../../shared/router/service/routing.facade';
import { WopiFacade } from '../../../../shared/wopi/services/wopi.facade';
import { selectEditTopic } from '../../../../shared/authorization/+state/selectors/authorization.selector';
import { Authorization } from '../../../../shared/authorization/models/authorization';
import { TopicTreeFacade } from '../../../splitscreen/components/tree-panel/services/topic-tree.facade';
import { SplitScreenFacade } from '../../../splitscreen/service/splitscreen.facade';
import { Role } from '../../../../shared/authorization/enums/role';
import { TopicResource } from '../../../../shared/topic/enums/topic-resource.enum';
import { ChartFacade } from '../../../../shared/chart/service/chart.facade';
import { TopicCharts } from '../../../../shared/chart/models/topicCharts';
import { FeatureflagFacade } from '../../../../shared/featureflag/service/featureflag.facade';
import { TooltipFeatures } from '../../../../shared/tooltip/enums/tooltipFeatures';
import { WopiTestFeatureflag } from '../../../wopi-test/constants';
import { WopiFeatures } from '../../../../shared/wopi/enums/wopi-feature-flags.enum';
import { TopicFacade } from '../../../../shared/topic/services/topic.facade';

@Component({
	selector: 'mav-topic',
	templateUrl: './topic.component.html',
	styleUrls: ['./topic.component.scss']
})
export class TopicComponent implements OnInit, OnDestroy {
	public constructor(
		private readonly store: Store,
		private readonly route: ActivatedRoute,
		private readonly routingFacade: RoutingFacade,
		private readonly errorService: ErrorService,
		private readonly chartFacade: ChartFacade,
		private readonly wopiFacade: WopiFacade,
		private readonly treeFacade: TopicTreeFacade,
		private readonly splitscreenFacade: SplitScreenFacade,
		private readonly featureflagFacade: FeatureflagFacade,
		private readonly topicFacade: TopicFacade
	) {}

	public topicData: Topic;
	public chartData: TopicCharts;
	public fieldData: Field[];
	public wopiUrl: string;
	public wopiActionUrls: WopiActionUrls;
	public relationData: Relation[];
	public subtopicsData: Topic[];
	public topicResource: TopicResource;
	public isAuthorizedToEdit: boolean;
	public get canEdit(): boolean {
		return this.isAuthorizedToEdit && !this.isTopicReadonly;
	}
	public get isTopicReadonly(): boolean {
		return this.topicData?.business?.isReadOnly ?? true;
	}

	public get wopiTest(): string {
		return WopiTestFeatureflag;
	}

	public authorizationData$: Observable<Authorization>;

	public isWopiApiEditNewEnabled: boolean;
	public tooltipFeatureFlag: boolean;
	public topicLoaded = false;
	public chartsLoaded = false;
	public fieldsLoaded = false;
	public relationsLoaded = false;
	public subTopicsLoaded = false;
	public errorState = false;
	public httpSuccessCode = 200;

	public dcvId: string;
	public get isWordButtonVisible(): boolean {
		return (
			(this.isWopiApiEditNewEnabled ||
				(this.topicData &&
					this.containsDescription(this.topicData.resources))) &&
			this.canEdit
		);
	}
	private readonly destroySubscription = new Subject();
	private readonly wopiLoadTimeout = 10;

	public ngOnInit(): void {
		const DCVID = 'dcvid';
		this.route.params.subscribe((params: Params) => {
			this.dcvId = params[DCVID] as string;
			this.cleanUpComponent();
			if (RegexUtils.dcvID().test(this.dcvId)) {
				this.initFeatureflags();
				this.initAuth();
				this.getWopiActionUrls();
				this.getBaseTopic(this.dcvId);
				this.getAuthorizationData();
			} else {
				this.errorState = true;
				this.errorService.handleClientError(
					'DcvID invalid',
					'routeParams'
				);
			}
		});
	}

	public ngOnDestroy(): void {
		this.destroySubscription.next();
		this.destroySubscription.complete();
	}

	public navigate(dcv: string): void {
		const { outlet } = this.route.snapshot;
		this.routingFacade.next(dcv, outlet === 'left' ? outlet : 'right');
	}

	public editNav(): void {
		this.routingFacade.edit(this.dcvId);
	}

	public editWordNav(): void {
		this.topicData && this.containsDescription(this.topicData.resources)
			? this.routingFacade.editWord(this.dcvId)
			: this.routingFacade.createNewWord(this.topicData);
	}

	public wopiTestNav(): void {
		this.routingFacade.testWopi(this.dcvId);
	}

	public isEmpty(data: unknown): boolean {
		return !data || data === undefined || data['length'] === 0;
	}

	public expandToTree(): void {
		this.treeFacade.ExpandTo(this.dcvId);
		this.splitscreenFacade.treePanelVisibility
			.pipe(take(1))
			.subscribe((isTreeVisible) => {
				if (!isTreeVisible) {
					this.splitscreenFacade.toggleTreePanelVisibility();
				}
			});
	}

	public containsChart(topicResources: TopicResource[]): boolean {
		return topicResources.includes(TopicResource.Chart);
	}

	public containsDescription(topicResources: TopicResource[]): boolean {
		return topicResources.includes(TopicResource.Description);
	}

	public getTooltipText(topicResources?: TopicResource[]): string {
		if (this.canCreateNewDocument(topicResources)) {
			return 'Create new Word document';
		}
		return 'Edit Word document';
	}

	public canCreateNewDocument(topicResources?: TopicResource[]): boolean {
		return (
			this.isWopiApiEditNewEnabled &&
			!this.containsDescription(topicResources)
		);
	}

	public containsFields(topicResources: TopicResource[]): boolean {
		return topicResources.includes(TopicResource.Fields);
	}

	public containsRelations(topicResources: TopicResource[]): boolean {
		return topicResources.includes(TopicResource.Relations);
	}

	public containsSubtopics(topicResources: TopicResource[]): boolean {
		return topicResources.includes(TopicResource.SubTopics);
	}

	private initAuth(): void {
		// adding delay gets rid of a know error "Expression has changed after it was checked"
		// https://blog.angular-university.io/angular-debugging/
		this.authorizationData$ = this.store
			.select(selectEditTopic)
			.pipe(delay(0));
	}

	private getWopiActionUrls(): void {
		let fetchWopiActionUrls = false;
		this.wopiFacade
			.getWopiActionUrls()
			.pipe(
				tap((wopiActionUrls) => {
					if (!wopiActionUrls && !fetchWopiActionUrls) {
						fetchWopiActionUrls = true;
						this.wopiFacade.loadWopiActionUrls();
					}
				}),
				takeUntil(this.destroySubscription)
			)
			.subscribe((wopiActionUrls) => {
				if (wopiActionUrls) {
					setTimeout(() => {
						this.wopiActionUrls = wopiActionUrls;
					}, this.wopiLoadTimeout);
				}
			});
	}

	private getBaseTopic(dcvId: string): void {
		let fetchTopic = false;
		this.store
			.select(selectTopicById(dcvId))
			.pipe(
				tap((topic) => {
					if (!topic?.resources && !fetchTopic) {
						fetchTopic = true;
						this.store.dispatch(
							topicActions.LoadTopicByDCV({ payload: dcvId })
						);
					}
				}),
				filter((topic) => !!topic?.resources),
				takeUntil(this.destroySubscription)
			)
			.subscribe((topic) => {
				setTimeout(() => {
					this.topicData = topic;
					this.topicLoaded = true;
				}, this.wopiLoadTimeout);

				this.errorState = topic.httpStatusCode !== this.httpSuccessCode;

				if (topic.resources.includes(TopicResource.Chart)) {
					this.getCharts(this.dcvId);
				}
				if (topic.resources.includes(TopicResource.Fields)) {
					this.getFields(this.dcvId);
				}
				if (topic.resources.includes(TopicResource.Relations)) {
					this.getRelations(this.dcvId);
				}
				if (topic.resources.includes(TopicResource.SubTopics)) {
					this.getSubTopics(this.dcvId);
				}
			});
	}

	private getCharts(dcvId: string): void {
		let fetchCharts = false;
		this.chartFacade
			.getCharts(dcvId)
			.pipe(delay(0))
			.pipe(
				tap((topicCharts) => {
					if (!topicCharts && !fetchCharts) {
						fetchCharts = true;
						this.chartFacade.loadCharts(dcvId);
					}
				}),
				filter((data) => !!data),
				takeUntil(this.destroySubscription)
			)
			.subscribe((topicCharts) => {
				this.chartData = topicCharts;
				this.chartsLoaded = true;
			});
	}

	private getFields(dcvId: string): void {
		let fetchFields = false;
		this.store
			.select(selectFieldByDcv(dcvId))
			.pipe(delay(0))
			.pipe(
				tap((fields) => {
					if ((!fields || fields.length === 0) && !fetchFields) {
						fetchFields = true;
						this.store.dispatch(
							fieldActions.LoadFields({ payload: dcvId })
						);
					}
				}),
				filter((data) => !!data),
				takeUntil(this.destroySubscription)
			)
			.subscribe((fields) => {
				this.fieldData = fields;
				this.fieldsLoaded = true;
			});
	}

	private getRelations(dcvId: string): void {
		let fetchRelations = false;
		this.store
			.select(selectRelationsByDcv(dcvId))
			.pipe(
				tap((relation) => {
					if (
						(!relation || relation.length === 0) &&
						!fetchRelations
					) {
						fetchRelations = true;
						this.store.dispatch(
							relationActions.LoadRelations({ payload: dcvId })
						);
					}
				}),
				takeUntil(this.destroySubscription)
			)
			.subscribe((relations) => {
				this.relationData = relations;
			});
	}

	private getSubTopics(dcvId: string): void {
		let fetchSubTopics = false;
		this.store
			.select(selectChildTopicsByDcv(dcvId))
			.pipe(
				tap((subTopic) => {
					if (
						(!subTopic || subTopic.length === 0) &&
						!fetchSubTopics
					) {
						fetchSubTopics = true;
						this.store.dispatch(
							topicActions.LoadTopicChildren({ payload: dcvId })
						);
					}
				}),
				takeUntil(this.destroySubscription)
			)
			.subscribe((subTopics) => {
				this.subtopicsData = subTopics;
				this.subTopicsLoaded = true;
			});
	}

	private getAuthorizationData(): void {
		this.authorizationData$
			.pipe(
				filter((data) => !!data),
				map((data) => data && data.role !== Role.Subscriber),
				takeUntil(this.destroySubscription)
			)
			.subscribe((authorizationData) => {
				this.isAuthorizedToEdit = authorizationData;
			});
	}

	// this is necessary because component is not destroyed.
	private cleanUpComponent(): void {
		this.destroySubscription.next();
		this.destroySubscription.complete();
		this.topicData = undefined;
		this.chartData = undefined;
		this.fieldData = undefined;
		this.wopiUrl = undefined;
		this.relationData = undefined;
		this.subtopicsData = undefined;
		this.isAuthorizedToEdit = false;
		this.topicLoaded = false;
		this.fieldsLoaded = false;
		this.relationsLoaded = false;
		this.subTopicsLoaded = false;
		this.errorState = false;
		this.tooltipFeatureFlag = false;
		this.isWopiApiEditNewEnabled = false;
	}

	private initFeatureflags(): void {
		this.featureflagFacade
			.getFeatureflag(TooltipFeatures.tooltip)
			.pipe(take(1))
			.subscribe((featureEnabled) => {
				this.tooltipFeatureFlag = featureEnabled;
			});
		this.featureflagFacade
			.getFeatureflag(WopiFeatures.wopiApiEditNew)
			.pipe(take(1))
			.subscribe((featureEnabled) => {
				this.isWopiApiEditNewEnabled = featureEnabled;
			});
	}
}
