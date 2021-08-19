import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import { UserAdministrationModule } from './container/user-administration/user-administration.module';
import { GroupAdministrationModule } from './container/group-administration/group-administration.module';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { reducer } from './container/group-administration/+state/reducers/reducers';
import { GroupEffects } from './container/group-administration/+state/effects/effects';

@NgModule({
	declarations: [],
	imports: [
		CommonModule,
		AdminRoutingModule,
		UserAdministrationModule,
		GroupAdministrationModule,
		StoreModule.forFeature('groups', reducer),
		EffectsModule.forFeature([GroupEffects])
	],
	providers: [],
	exports: []
})
export class AdminModule {}
