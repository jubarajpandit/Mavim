import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserAdministrationComponent } from './user-administration.component';
import { AddUserButtonComponent } from '../../shared/components/button/add-user-button/add-user-button.component';
import { AddUserComponent } from './components/add-user/add-user.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ComponentsModule } from '../../../../shared/components/components.module';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { AdminAuthorizedEffects } from './+state/effects/effects';
import { reducer as adminReducer } from './+state/reducers/reducers';
import { FeatureRoutingModule as UserAdministrationRoutingModule } from './user-administration-routing.module';
import { UsersResolver } from './service/admin-users.resolver';
import { AdminAuthorizationService } from './service/admin-user.service';
import { AdminUserFacade } from './service/admin-user.facade';
import { LoadersModule } from '../../../../shared/loaders/loaders.module';
import { EditUserComponent } from './components/edit-user/edit-user.component';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { DeleteUserComponent } from './components/delete-user/delete-user.component';
import { EditUserButtonComponent } from '../../shared/components/button/edit-user-button/edit-user-button.component';
import { DeleteUserButtonComponent } from '../../shared/components/button/delete-user-button/delete-user-button.component';
import { DeleteUserTemplateComponent } from './components/delete-user-template/delete-user-template.component';
import { ModalFactoryModule } from '../../../../shared/modal/components/modalfactory/modalfactory.module';
import { CloseButtonComponent } from '../../shared/components/button/close-button/close-button.component';

@NgModule({
	declarations: [
		UserAdministrationComponent,
		AddUserComponent,
		EditUserComponent,
		AddUserButtonComponent,
		EditUserButtonComponent,
		DeleteUserButtonComponent,
		DeleteUserComponent,
		DeleteUserTemplateComponent,
		CloseButtonComponent
	],
	imports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		ComponentsModule,
		ModalFactoryModule,
		UserAdministrationRoutingModule,
		LoadersModule,
		ScrollingModule,
		StoreModule.forFeature('adminUsers', adminReducer),
		EffectsModule.forFeature([AdminAuthorizedEffects])
	],
	exports: [],
	providers: [UsersResolver, AdminUserFacade, AdminAuthorizationService]
})
export class UserAdministrationModule {}
