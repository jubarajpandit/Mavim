import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthorizationGuard } from '../shared/authorization/guard/authorization.guard';
import { LayoutRoute } from '../shared/layout/interfaces';
import { DatabaseGuard } from '../shared/database/guard/database.guard';
import { MsalGuard } from '@azure/msal-angular';
import { Language } from '../shared/language/enums/language.enum';
import { Role } from '../shared/authorization/enums/role';
import { PageNotFoundComponent } from '../shared/components/page-not-found/page-not-found.component';
import { FeatureflagGuard } from '../shared/featureflag/guard/featureflag.guard';
import { WopiTestFeatureflag } from '../containers/wopi-test/constants';

export const allowedLanguageRoutes = (children: LayoutRoute): LayoutRoute =>
	Object.values(Language).map((path) => ({ path, children }));

const appRoutes: LayoutRoute = [
	{
		path: '',
		loadChildren: () =>
			import('../containers/init/init.module').then((m) => m.InitModule),
		canActivate: [MsalGuard]
	},
	...allowedLanguageRoutes([
		{
			path: '',
			loadChildren: () =>
				import('../containers/splitscreen/splitscreen.module').then(
					(m) => m.SplitScreenModule
				),
			canActivate: [MsalGuard, DatabaseGuard, AuthorizationGuard],
			data: {
				roles: [Role.Subscriber, Role.Contributor, Role.Administrator]
			}
		},
		{
			path: 'edit',
			loadChildren: () =>
				import('../containers/edit/edit.module').then(
					(m) => m.EditContainerModule
				),
			canActivate: [MsalGuard, DatabaseGuard, AuthorizationGuard],
			data: {
				roles: [Role.Contributor, Role.Administrator]
			}
		},
		{
			path: 'new/word',
			loadChildren: () =>
				import('../containers/word/newword/newword.module').then(
					(m) => m.NewWordContainerModule
				),
			canActivate: [MsalGuard, DatabaseGuard, AuthorizationGuard],
			data: {
				roles: [Role.Contributor, Role.Administrator]
			}
		}
	]),
	{
		path: 'test',
		loadChildren: () =>
			import('../containers/wopi-test/wopitest.module').then(
				(m) => m.WopiTestContainerModule
			),
		canActivate: [FeatureflagGuard],
		data: {
			featureflag: WopiTestFeatureflag,
			fullscreen: true,
			icon: 'test'
		}
	},
	{
		path: 'admin',
		loadChildren: () =>
			import('../containers/admin/admin.module').then(
				(m) => m.AdminModule
			),
		canActivate: [MsalGuard, DatabaseGuard, AuthorizationGuard],
		data: {
			roles: [Role.Administrator]
		}
	},
	{
		path: '**',
		component: PageNotFoundComponent
	}
];

@NgModule({
	imports: [RouterModule.forRoot(appRoutes, { enableTracing: false })],
	exports: [RouterModule]
})
export class AppRoutingModule {}
