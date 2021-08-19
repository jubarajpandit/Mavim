import {
	Directive,
	Input,
	OnDestroy,
	OnInit,
	TemplateRef,
	ViewContainerRef
} from '@angular/core';
import { Subject } from 'rxjs';
import { filter, mergeMap, takeUntil, tap } from 'rxjs/operators';
import { FetchStatus } from '../../enums/FetchState';
import { FeatureflagFacade } from '../service/featureflag.facade';

@Directive({
	// eslint-disable-next-line @angular-eslint/directive-selector
	selector: '[featureFlag]'
})
export class FeatureflagDirective<T = unknown> implements OnInit, OnDestroy {
	public constructor(
		private readonly templateRef: TemplateRef<T>,
		private readonly viewContainer: ViewContainerRef,
		private readonly featureFlagFacade: FeatureflagFacade
	) {}

	@Input() public featureFlag: string;

	private readonly onDestroy = new Subject<void>();

	public ngOnInit(): void {
		const featureflagSubscription = this.featureFlagFacade
			.getFeatureflag(this.featureFlag)
			.pipe(
				tap((isEnabled) => {
					if (isEnabled) {
						this.viewContainer.createEmbeddedView(this.templateRef);
					} else {
						this.viewContainer.clear();
					}
					this.destroy();
				}),
				takeUntil(this.onDestroy)
			);

		this.featureFlagFacade.fetchFeatureflags
			.pipe(
				filter((status) => status === FetchStatus.Fetched),
				mergeMap(() => featureflagSubscription),
				takeUntil(this.onDestroy)
			)
			.subscribe();
	}

	public ngOnDestroy(): void {
		this.destroy();
	}

	private destroy(): void {
		this.onDestroy.next();
		this.onDestroy.complete();
	}
}
