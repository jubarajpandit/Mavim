import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { chartReducer } from './+state/reducers/chart.reducers';
import { ChartEffects } from './+state/effects/chart.effects';
import { ChartService } from './service/chart.service';
import { ChartFacade } from './service/chart.facade';

@NgModule({
	declarations: [],
	imports: [
		CommonModule,
		StoreModule.forFeature('charts', chartReducer),
		EffectsModule.forFeature([ChartEffects])
	],
	providers: [ChartService, ChartFacade]
})
export class ChartModule {}
