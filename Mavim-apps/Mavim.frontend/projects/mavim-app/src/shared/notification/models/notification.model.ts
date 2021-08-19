import { NotificationTypes } from '../enums/notification-types.enum';

export class Notification {
	public actions: string[];
	public message: string;
	public type: NotificationTypes;
}
