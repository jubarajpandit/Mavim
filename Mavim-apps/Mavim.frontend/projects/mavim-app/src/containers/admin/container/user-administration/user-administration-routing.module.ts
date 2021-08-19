import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { UserAdministrationComponent } from './user-administration.component';
import { UsersResolver } from './service/admin-users.resolver';
import { AddUserComponent } from './components/add-user/add-user.component';
import { EditUserComponent } from './components/edit-user/edit-user.component';

const routes: Routes = [
	{
		path: '',
		component: UserAdministrationComponent,
		resolve: {
			users: UsersResolver
		},
		children: [
			{
				path: 'add',
				component: AddUserComponent
			},
			{
				path: 'edit/:userid',
				component: EditUserComponent
			}
		]
	}
];

@NgModule({
	imports: [CommonModule, RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class FeatureRoutingModule {}
