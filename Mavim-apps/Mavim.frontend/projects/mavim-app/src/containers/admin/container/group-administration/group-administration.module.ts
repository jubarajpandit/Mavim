import { NgModule } from '@angular/core';
import { LoadersModule } from '../../../../shared/loaders/loaders.module';
import { FeatureGroupRoutingModule as GroupAdministrationRoutingModule } from './group-administration-routing.module';
import { ComponentsModule } from '../../../../shared/components/components.module';
import { CommonModule } from '@angular/common';
import { CreateGroupComponent } from './components/create-group/create-group.component';
import { GroupAdministrationComponent } from './group-administration.component';
import { GroupsOverviewComponent } from './components/groups-overview/groups-overview.component';
import { GroupCardComponent } from './components/group-card/groups-card.component';
import { IconsModule } from '../../../../shared/icons/icons.module';

@NgModule({
	declarations: [
		GroupAdministrationComponent,
		CreateGroupComponent,
		GroupsOverviewComponent,
		GroupCardComponent
	],
	imports: [
		CommonModule,
		ComponentsModule,
		GroupAdministrationRoutingModule,
		LoadersModule,
		IconsModule
	]
})
export class GroupAdministrationModule {}
