import { NgModule, ModuleWithProviders } from '@angular/core';
import {
	RoutingService,
	RoutingSessionStoreService,
	RoutingBrowserService
} from './service';
import { RoutingFacade } from './service/routing.facade';
import { EffectsModule } from '@ngrx/effects';
import { RoutingEffect } from './+state/router.effects';
import { StoreModule } from '@ngrx/store';
import { reducer } from './+state/router.reducer';
import { WINDOW_PROVIDERS } from './service/window.provider';

const commonProviders = [
	RoutingFacade,
	WINDOW_PROVIDERS,
	RoutingService,
	RoutingEffect,
	RoutingSessionStoreService
];

@NgModule({
	imports: [
		StoreModule.forFeature('routing', reducer),
		EffectsModule.forFeature([RoutingEffect])
	]
})
export class RoutingModule {
	public static forRoot(): ModuleWithProviders<RoutingModule> {
		return {
			ngModule: RoutingModule,
			providers: [...commonProviders, RoutingBrowserService]
		};
	}
	public static forChild(): ModuleWithProviders<RoutingModule> {
		return {
			ngModule: RoutingModule,
			providers: [...commonProviders]
		};
	}
}
