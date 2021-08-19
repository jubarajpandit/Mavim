import { createAction, props } from '@ngrx/store';
import { Notification } from '../../models/notification.model';

export const AddNotification = createAction(
	'[Notification] Add Notification',
	props<{ payload: Notification }>()
);
export const CloseLastNotification = createAction(
	'[Notification] Close Last Notification'
);
