import { TestBed } from '@angular/core/testing';
import { NotificationService } from './notification.service';
import { Store } from '@ngrx/store';
import { MockStore } from '@ngrx/store/testing';
import { NotificationTypes } from '../enums/notification-types.enum';
import { Notification } from '../models/notification.model';

describe('NotificatationService', () => {
  let service: NotificationService;
  let notificationStore: MockStore<{ notifications: [] }>;

  const storeMock = {
    dispatch(payload) {
      return jest.fn();
    },
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: Store,
          useValue: storeMock,
        },
        NotificationService,
      ],
    }).compileComponents();

    notificationStore = TestBed.get(Store);
    service = TestBed.get(NotificationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('sendErrorNotification() should dispatch action in notification store when called', () => {
    const notificationStoreSpy = spyOn<any>(notificationStore, 'dispatch');
    const type: NotificationTypes = NotificationTypes.Error;

    service.sendNotification(type, 'test');

    expect(notificationStoreSpy).toHaveBeenCalled();
  });

  it('closeNotification() should dispatch action in notification store when called', () => {
    const notificationStoreSpy = spyOn<any>(notificationStore, 'dispatch');
    const method = 'getNotificationByType';
    const notification: Notification = service[method](NotificationTypes.Error, 'test');

    service.closeNotification(notification);

    expect(notificationStoreSpy).toHaveBeenCalled();
  });
});
