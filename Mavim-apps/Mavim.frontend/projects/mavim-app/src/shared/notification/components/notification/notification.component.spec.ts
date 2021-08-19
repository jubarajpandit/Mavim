import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationComponent } from './notification.component';
import { ButtonComponent } from '../../../controls/components/button/button.component';
import { of } from 'rxjs';
import { NotificationTypes } from '../../enums/notification-types.enum';

describe('NotificationComponent', () => {
  let component: NotificationComponent;
  let fixture: ComponentFixture<NotificationComponent>;
  const notifications = [];
  const testNotification = {
    type: NotificationTypes.Success,
    message: 'test',
    closed: false,
    actions: ['close'],
    guid: 'xxx',
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NotificationComponent, ButtonComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotificationComponent);
    component = fixture.componentInstance;
    component.notifications$ = of(notifications);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('getClassByType() should return the correct class by enum when called with type', () => {
    const type: NotificationTypes = NotificationTypes.Error;
    const expectedValue = 'error';

    const actualValue = component.getClassByType(type);

    expect(actualValue).toEqual(expectedValue);
  });

  it('closeLastNotificationAutoIfSuccess() should emit close action if success notification is the last open notification', () => {
    jest.useFakeTimers();
    const spy = jest.spyOn(component.action, 'emit');
    component.notifications = [testNotification, testNotification];

    component.closeLastNotificationAutoIfSuccess();

    jest.runOnlyPendingTimers();

    expect(spy).toHaveBeenCalled();
  });
});
