import { NgModule } from '@angular/core';
import { StoreModule } from '@ngrx/store';
import { notificationReducer } from './+state/reducers/notification.reducers';
import { CommonModule } from '@angular/common';
import { NotificationComponent } from './components/notification/notification.component';
import { ComponentsModule } from '../components/components.module';

const NotificationModuleComponents = [NotificationComponent];

@NgModule({
	imports: [
		CommonModule,
		StoreModule.forFeature('notifications', notificationReducer),
		ComponentsModule
	],
	exports: [...NotificationModuleComponents],
	declarations: [...NotificationModuleComponents]
})
export class NotificationsModule {}
