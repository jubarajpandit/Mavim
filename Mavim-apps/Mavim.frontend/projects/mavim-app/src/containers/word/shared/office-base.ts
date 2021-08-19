import { Store } from '@ngrx/store';
import { Subject } from 'rxjs';
import { tap, takeUntil, filter, mergeMap, take } from 'rxjs/operators';
import { Directive, OnInit, OnDestroy } from '@angular/core';
import { WopiActionUrls } from '../../../shared/wopi/models/wopi-actionurls.model';
import { WopiFacade } from '../../../shared/wopi/services/wopi.facade';
import { RegexUtils } from '../../../shared/utils';
import { ErrorService } from '../../../shared/notification/services/error.service';
import { FetchStatus } from '../../../shared/enums/FetchState';
import { selectTopicById } from '../../../shared/topic/+state/selectors/topic.selector';
import * as topicActions from '../../../shared/topic/+state/actions/topic.actions';
import { TopicResource } from '../../../shared/topic/enums/topic-resource.enum';

@Directive()
export abstract class OfficeBase implements OnInit, OnDestroy {
	public constructor(
		protected readonly store: Store,
		protected readonly errorService: ErrorService,
		protected readonly wopiFacade: WopiFacade
	) {}

	public dcvId: string;
	public isResourceExisting: boolean;
	public wopiActionUrls: WopiActionUrls;
	protected resource: TopicResource;

	private readonly wopiLoadTimeout = 10;
	private readonly destroySubscription = new Subject();

	public ngOnInit(): void {
		if (RegexUtils.dcvID().test(this.dcvId)) {
			this.getWopiActionUrls();
			this.getTopicResource();
		} else {
			this.errorService.handleClientError('DcvID invalid', 'routeParams');
		}
	}

	public ngOnDestroy(): void {
		this.destroySubscription.next();
		this.destroySubscription.complete();
	}

	protected getWopiActionUrls(): void {
		let fetchWopiActionUrls = false;
		this.wopiFacade
			.getFetchWopiActionUrls()
			.pipe(
				tap((fetched) => {
					if (
						fetched === FetchStatus.NotFetched &&
						!fetchWopiActionUrls
					) {
						fetchWopiActionUrls = true;
						this.wopiFacade.loadWopiActionUrls();
					}
				}),
				filter((status) => status === FetchStatus.Fetched),
				mergeMap(() =>
					this.wopiFacade.getWopiActionUrls().pipe(
						filter((urls) => !!urls),
						take(1)
					)
				),
				take(1)
			)
			.subscribe((wopiActionUrls) => {
				if (wopiActionUrls) {
					setTimeout(() => {
						this.wopiActionUrls = wopiActionUrls;
					}, this.wopiLoadTimeout);
				}
			});
	}

	protected getTopicResource(): void {
		let fetchTopic = false;
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
				takeUntil(this.destroySubscription)
			)
			.subscribe((topic) => {
				if (topic) {
					this.isResourceExisting = topic.resources.includes(
						this.resource
					);
				}
			});
	}
}
