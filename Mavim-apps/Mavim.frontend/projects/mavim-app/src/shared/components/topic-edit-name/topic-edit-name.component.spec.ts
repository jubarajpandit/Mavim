import { async, ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { TopicEditNameComponent } from './topic-edit-name.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { FormValidationError } from '../../models/formvalidationerror.model';

describe('TopicTreeComponent', () => {
  let component: TopicEditNameComponent;
  let fixture: ComponentFixture<TopicEditNameComponent>;
  const inputValue = 'value';

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TopicEditNameComponent],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopicEditNameComponent);
    component = fixture.componentInstance;
    component.titleText = inputValue;
    fixture.detectChanges();
  });

  function arrange_spyOn_topicNameEditError() {
    spyOn(component.topicNameEditError, 'emit');
  }

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should pass topicName input parameter to the name formcontrol', () => {
    const actualValue = component.titleTextFormControl.value;

    expect(actualValue).toEqual(inputValue);
  });

  it('should emit nameChanged on change if form is valid', fakeAsync(() => {
    spyOn(component.nameChanged, 'emit');
    const changedValue = 'hello';

    component.titleTextFormControl.setValue(changedValue);
    tick(300);
    fixture.detectChanges();

    expect(component.nameChanged.emit).toHaveBeenCalledWith(changedValue);
  }));

  describe('should emit topicNameEditError on change if form is invalid', () => {
    const componentName = 'TopicEditNameComponent';
    const expectedValue: FormValidationError = new FormValidationError();
    expectedValue.componentName = componentName;

    it('when minlength condition is not satisfied', fakeAsync(() => {
      arrange_spyOn_topicNameEditError();
      const changedValue = 'he';
      const expectedErrorType = 'minlength';
      expectedValue.errorType = expectedErrorType;

      component.titleTextFormControl.setValue(changedValue);
      tick(300);
      fixture.detectChanges();

      expect(component.topicNameEditError.emit).toHaveBeenCalledWith(expectedValue);
    }));

    it('when required condition is not satisfied', fakeAsync(() => {
      arrange_spyOn_topicNameEditError();
      const changedValue = '';
      const expectedErrorType = 'required';
      expectedValue.errorType = expectedErrorType;

      component.titleTextFormControl.setValue(changedValue);
      tick(300);
      fixture.detectChanges();

      expect(component.topicNameEditError.emit).toHaveBeenCalledWith(expectedValue);
    }));
  });
});
