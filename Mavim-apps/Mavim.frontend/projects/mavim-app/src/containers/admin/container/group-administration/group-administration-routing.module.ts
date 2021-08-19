import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { GroupAdministrationComponent } from './group-administration.component';
import { CreateGroupComponent } from '../group-administration/components/create-group/create-group.component';
import { GroupsResolver } from './service/group.resolver';

const routes: Routes = [
	{
		path: '',
		resolve: {
			groups: GroupsResolver
		},
		component: GroupAdministrationComponent,
		children: [
			{
				path: 'add',
				component: CreateGroupComponent
			}
		]
	}
];

@NgModule({
	imports: [CommonModule, RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class FeatureGroupRoutingModule {}
