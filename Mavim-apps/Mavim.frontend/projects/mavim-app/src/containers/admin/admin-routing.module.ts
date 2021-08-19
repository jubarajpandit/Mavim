import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FeatureflagGuard } from '../../shared/featureflag/guard/featureflag.guard';
import { LayoutRoute } from '../../shared/layout/interfaces';
import { AdminGroupPage } from './container/group-administration/enums/featureflag';

const routes: LayoutRoute = [
	{
		path: '',
		redirectTo: 'users',
		pathMatch: 'full'
	},
	{
		path: 'users',
		loadChildren: () =>
			import(
				'./container/user-administration/user-administration.module'
			).then((m) => m.UserAdministrationModule)
	},
	{
		path: 'groups',
		loadChildren: () =>
			import(
				'./container/group-administration/group-administration.module'
			).then((m) => m.GroupAdministrationModule),
		canActivate: [FeatureflagGuard],
		data: {
			featureflag: AdminGroupPage.adminGroupPage
		}
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class AdminRoutingModule {}
