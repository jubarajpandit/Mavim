import {
	Component,
	OnInit,
	Input,
	Output,
	EventEmitter,
	DoCheck
} from '@angular/core';
import { Notification } from '../../models/notification.model';
import { NotificationTypes } from '../../enums/notification-types.enum';
import { Observable } from 'rxjs';

@Component({
	selector: 'mav-notification',
	templateUrl: './notification.component.html',
	styleUrls: ['./notification.component.scss']
})
export class NotificationComponent implements OnInit, DoCheck {
	@Input() public notifications$: Observable<Notification[]>;
	@Output() public action = new EventEmitter<Notification>();
	public allClosed: boolean;
	public notifications: Notification[];

	private readonly timeOut = 3000;

	public checkIfAllClosed(): void {
		this.allClosed = this.notifications?.length === 0;
	}

	public get lastNotification(): Notification {
		return this.notifications?.slice(-1)[0];
	}

	public closeLastNotificationAutoIfSuccess(): void {
		if (this.notifications && this.notifications.length) {
			const lastNotification =
				this.notifications[this.notifications.length - 1];
			if (lastNotification.type === NotificationTypes.Success) {
				setTimeout(() => {
					this.action.emit(lastNotification);
				}, this.timeOut);
			}
		}
	}

	public getClassByType(type: NotificationTypes): string {
		return NotificationTypes[type].toLowerCase();
	}

	public ngDoCheck(): void {
		this.checkIfAllClosed();
	}

	public ngOnInit(): void {
		this.notifications$.subscribe((notifications: Notification[]) => {
			this.notifications = notifications;
			this.closeLastNotificationAutoIfSuccess();
		});
	}

	public numberOfOpenNotifications(): number {
		const openNotification = this.notifications;
		return openNotification.length;
	}

	public onClick(notification: Notification): void {
		this.action.emit(notification);
	}
}
