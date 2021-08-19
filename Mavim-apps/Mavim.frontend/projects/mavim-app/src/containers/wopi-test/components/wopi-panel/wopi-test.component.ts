import { WopiActionUrls } from '../../../../shared/wopi/models/wopi-actionurls.model';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { RegexUtils } from '../../../../shared/utils';
import { ErrorService } from '../../../../shared/notification/services/error.service';
import { Wopi } from '../../../../shared/wopi/models/wopi.model';
import { WopiFacade } from '../../../../shared/wopi/services/wopi.facade';
import { filter, mergeMap, take, takeUntil, tap } from 'rxjs/operators';
import { FetchStatus } from '../../../../shared/enums/FetchState';

@Component({
	selector: 'mav-wopi-test',
	templateUrl: './wopi-test.component.html',
	styleUrls: ['./wopi-test.component.scss']
})
export class WopiTestComponent implements OnInit {
	public constructor(
		private readonly route: ActivatedRoute,
		private readonly errorService: ErrorService,
		private readonly wopiFacade: WopiFacade
	) {
		const dcvid = this.route.snapshot.params['dcvid'] as string;
		this.dcvId = dcvid;
	}

	public readonly dcvId: string;
	public wopiMetaData: Wopi;
	public wopiActionUrls: WopiActionUrls;

	private readonly wopiLoadTimeout = 10;

	public ngOnInit(): void {
		if (RegexUtils.dcvID().test(this.dcvId)) {
			this.getWopiActionUrls();
		} else {
			this.errorService.handleClientError('DcvID invalid', 'routeParams');
		}
	}

	private getWopiActionUrls(): void {
		const destroySubscription = new Subject();
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
						take(1),
						filter((urls) => !!urls)
					)
				),
				takeUntil(destroySubscription)
			)
			.subscribe((wopiActionUrls) => {
				if (wopiActionUrls) {
					setTimeout(() => {
						this.wopiActionUrls = wopiActionUrls;
						destroySubscription.next();
						destroySubscription.complete();
					}, this.wopiLoadTimeout);
				}
			});
	}
}
