import { APP_INITIALIZER, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EffectsModule } from '@ngrx/effects';
import { FeatureflagEffects } from './+state/effects/featureflag.effects';
import { Store, StoreModule } from '@ngrx/store';
import { FeatureflagFacade } from './service/featureflag.facade';
import { reducer as featureflagReducer } from './+state';
import { FeatureflagService } from './service/featureflag.service';
import { FeatureflagDirective } from './components/featureflag.directive';

export function initFeatureflag(store: Store): () => void {
	return (): void => {
		new FeatureflagFacade(store).loadFeatureflags();
	};
}

const components = [FeatureflagDirective];

@NgModule({
	declarations: [components],
	imports: [
		CommonModule,
		StoreModule.forFeature('featureflag', featureflagReducer),
		EffectsModule.forFeature([FeatureflagEffects])
	],
	exports: [components],
	providers: [
		FeatureflagService,
		{
			provide: APP_INITIALIZER,
			useFactory: initFeatureflag,
			deps: [Store],
			multi: true
		}
	]
})
export class FeatureflagModule {}
