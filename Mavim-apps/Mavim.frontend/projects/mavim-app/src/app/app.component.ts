import { Component, OnInit } from '@angular/core';
import { environment } from '../environments/environment';
import { getNotifications } from '../shared/notification/+state';
import { Notification } from '../shared/notification/models/notification.model';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { NotificationService } from '../shared/notification/services/notification.service';
import { filter } from 'rxjs/operators';
import { LayoutFacade } from '../shared/layout/service/layout.facade';
import { TopicFacade } from '../shared/topic/services/topic.facade';

@Component({
	selector: 'mav-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
	public constructor(
		private readonly store: Store,
		private readonly notificationService: NotificationService,
		private readonly topicFacade: TopicFacade,
		private readonly layoutFacade: LayoutFacade
	) {
		this.notification$ = this.store.select(getNotifications);
	}

	public notification$: Observable<Notification[]>;
	public rootLoaded = false;
	public isFullscreen = true;
	public title = 'Mavim Manager Web';
	public version: string = environment.VERSION;

	public ngOnInit(): void {
		this.topicFacade.loadRoot();
		this.topicFacade.loadCategories();
		this.layoutFacade.layoutState
			.pipe(filter((layout) => layout !== undefined))
			.subscribe((layout) => {
				this.isFullscreen = layout;
			});
	}

	public closeNotification(): void {
		this.notificationService.closeLastNotification();
	}
}
