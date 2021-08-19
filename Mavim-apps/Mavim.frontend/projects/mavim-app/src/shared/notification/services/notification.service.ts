import { Injectable } from '@angular/core';
import * as NotificationActions from '../+state/actions/notification.actions';
import { Store } from '@ngrx/store';
import { Notification } from '../models/notification.model';
import { NotificationTypes } from '../enums/notification-types.enum';

@Injectable({
	providedIn: 'root'
})
export class NotificationService {
	public constructor(private readonly store: Store) {}

	public closeLastNotification(): void {
		this.store.dispatch(NotificationActions.CloseLastNotification());
	}

	public sendNotification(type: NotificationTypes, message: string): void {
		const notification: Notification = this.getNotificationByType(
			type,
			message
		);

		this.dispatchNotification(notification);
	}

	private dispatchNotification(notification: Notification): void {
		this.store.dispatch(
			NotificationActions.AddNotification({ payload: notification })
		);
	}

	private getNotificationByType(
		type: NotificationTypes,
		message: string
	): Notification {
		return {
			type,
			message,
			actions: ['close']
		};
	}
}
