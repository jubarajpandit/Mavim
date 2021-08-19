/* eslint-disable dot-notation, @typescript-eslint/dot-notation */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { EditPanelComponent } from './edit-panel.component';
import { FormValidationError } from '../../models/formvalidationerror.model';
import { Store } from '@ngrx/store';
import { MockStore } from '@ngrx/store/testing';
import { of, Subject } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';
import { TopicEditNameComponent } from '../topic-edit-name/topic-edit-name.component';
import { ReactiveFormsModule } from '@angular/forms';
import { FieldsEditComponent } from '../../../field/components/fields-edit/fields-edit.component';
import { RelationsEditComponent } from '../../../relation/components/relations-edit/relations-edit.component';
import { DevExtremeModule } from 'devextreme-angular';
import { NotificationService } from '../../../notification/services/notification.service';
import { ButtonComponent } from '../../../controls/components/button/button.component';
import { DotLoaderComponent } from '../../../loaders/dot-loader/dot-loader.component';
import { ModalComponent } from '../../../modal/components/modal/modal.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { EDITPANEL_FACADE } from './tokens/edit-panel.token';
import { IEditPanelFacade } from './interfaces/iedit-panel.facade';
import { Field } from '../../../field/models/field.model';
import { EditRelation } from '../../../relation/models/edit-relation.model';
import { Topic } from '../../../topic/models/topic.model';
import { EditStatus } from '../../enums/edit-status.enum';
import { EditTopic } from '../../../topic/models/edit-topic.model';
import { EditField } from '../../../field/models/edit-field.model';

describe('EditPanelComponent', () => {
  let component: EditPanelComponent;
  let fixture: ComponentFixture<EditPanelComponent>;
  let store: MockStore<{}>;
  let topicStore: MockStore<{ topics: []; splitScreen: { fullEditState: 'edit' } }>;
  let notificationStore: MockStore<{ notifications: [] }>;
  let fieldStore: MockStore<{ fields: [] }>;
  let relationStore: MockStore<{ relations: [] }>;
  let notificationService: NotificationService;
  const unsubscribe = new Subject<void>();

  class MockEditPanelFacade implements IEditPanelFacade {
    updateFieldValues(newFieldValue: Field): Promise<void> {
      return;
    }
  }

  const emptyEntityresponse = {
    entities: {},
    ids: [],
    subscribe() {
      return true;
    },
  };

  const notificationMockService = {
    sendNotification() {
      return jest.fn();
    },
  };

  const modalMockService = {
    init() {
      return jest.fn();
    },
    destroy() {
      return jest.fn();
    },
  };

  const storeMock = {
    select() {
      return of(emptyEntityresponse);
    },
    dispatch(payload) {
      return jest.fn();
    },
  };

  const testFieldData = {
    fieldID: 'd5926266c1352v0_d5926266c1353v0',
    fieldSetID: 'test',
    setName: 'General',
    fieldName: 'Status',
    fieldValue: ['Status filled'],
    fieldValueType: 'Text',
    IsMultiValue: false,
    IsMultiLingual: false,
    IsMultiLine: false,
    topicDCV: 'test',
  };

  const relationData: EditRelation = {
    category: 'category',
    characteristic: new Topic(),
    categoryType: 'categoryType',
    status: EditStatus.Unchanged,
    dcv: 'd5926266c1352v0',
    dispatchInstructions: new Array(),
    icon: '',
    topicDCV: 'd5926266c1353v0',
    userInstruction: new Topic(),
    withElement: new Topic(),
    withElementParent: new Topic(),
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        EditPanelComponent,
        DotLoaderComponent,
        TopicEditNameComponent,
        FieldsEditComponent,
        RelationsEditComponent,
        ButtonComponent,
        ModalComponent,
      ],
      imports: [RouterTestingModule, ReactiveFormsModule, DevExtremeModule],
      providers: [
        {
          provide: Store,
          useValue: storeMock,
        },
        {
          provide: NotificationService,
          useValue: notificationMockService,
        },
        {
          provide: EDITPANEL_FACADE,
          useClass: MockEditPanelFacade,
        },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditPanelComponent);
    component = fixture.componentInstance;
    store = TestBed.get(Store);
    topicStore = TestBed.get(Store);
    fieldStore = TestBed.get(Store);
    relationStore = TestBed.get(Store);
    notificationStore = TestBed.get(Store);
    notificationService = TestBed.get(NotificationService);
    fixture.detectChanges();
  });

  afterEach(() => {
    unsubscribe.next();
    unsubscribe.complete();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('onValidationError() should register a validation error when fired on validation from a component', () => {
    const expectedError = new FormValidationError();
    component.validationErrors = {};
    expectedError.componentName = 'fields';
    expectedError.errorType = 'required';

    component.onValidationError(expectedError, expectedError.componentName);

    const actualValidationError = component.validationErrors[expectedError.componentName];

    expect(actualValidationError).toEqual(expectedError);
    expect(Object.keys(component.validationErrors).length).toBeGreaterThan(0);
  });

  it('closeEdited() should reset changes, errors, edited data and emit close output', () => {
    const emitSpy = jest.spyOn(component.closeFullEdit, 'emit');

    component.closeEdited();

    expect(emitSpy).toHaveBeenCalled();
  });

  it('handleTopicChanged() should set data in updatedTopic property', () => {
    const expectedData = { dcv: undefined, name: 'name', editStatus: EditStatus.Updated } as EditTopic;

    component.handleTopicChanged(expectedData.name, '');

    expect(component['updatedTopic']).toEqual(expectedData);
  });

  describe('saving should', () => {
    it('dispatch an action in the topic store when saveTopic() is called', () => {
      const topicStoreSpy = spyOn<any>(component['topicStore'], 'dispatch');
      component['updatedTopic'] = { dcv: undefined, name: 'name', editStatus: EditStatus.Updated } as EditTopic;

      component['saveTopic']();

      expect(topicStoreSpy).toHaveBeenCalled();
    });

    it('dispatch an action in the field store when saveFields() is called', () => {
      const fieldStoreSpy = spyOn<any>(component['editPanelFacade'], 'updateFieldValues');
      const field: EditField = { data: ['test'], status: EditStatus.Updated } as EditField;
      component['updatedFields'] = [field];

      component['saveFields']();

      expect(fieldStoreSpy).toHaveBeenCalled();
    });
  });

  it('showValidationErrors() send error notification when called with error details', () => {
    const spySendNotification = spyOn<any>(notificationService, 'sendNotification');

    component['showValidationErrors']();

    expect(spySendNotification).toHaveBeenCalled();
  });

  describe('hasOpenChanges() should', () => {
    it('return true when changes are found', () => {
      component.handleTopicChanged('test', 'title');

      const expectedValue: boolean = component['hasOpenChanges']();

      expect(expectedValue).toEqual(true);
    });
    it('return false when no changes are found', () => {
      const expectedValue: boolean = component['hasOpenChanges']();

      expect(expectedValue).toBeFalsy();
    });
  });

  describe('saveEdited() should', () => {
    it('send a notification when there are open errors', () => {
      const spySendEditNotification = spyOn<any>(notificationService, 'sendNotification');
      const openErrors = { 'max-length': new FormValidationError() };
      component.validationErrors = openErrors;

      component['trySaveChanges']();

      expect(spySendEditNotification).toHaveBeenCalled();
    });
    it('save valid keys when there are no open errors', () => {
      const spyOnSave = spyOn<any>(component, 'saveAll');
      component.validationErrors = {};
      component['hasOpenChanges'] = jest.fn(() => true);
      component['closeEditWhenSavingIsComplete'] = jest.fn();

      component['trySaveChanges']();

      expect(spyOnSave).toHaveBeenCalled();
    });
    it('set showConfirmClose to true when there are no open changes', () => {
      component['hasOpenChanges'] = jest.fn(() => true);

      component['cancelEdited']();

      expect(component['showConfirmClose']).toBeTruthy();
    });
  });

  describe('handleDeleteRelationEvent() should', () => {
    it('should set status to deleted when incoming relation has status unchanged', () => {
      component['handleDeleteRelationEvent'](relationData);

      expect(relationData.status).toEqual(EditStatus.Deleted);
    });

    it('save valid keys when there are no open errors', () => {
      const spyOnDeleteSoftSavedRelation = spyOn<any>(component, 'deleteSoftSavedRelation');
      relationData.status = EditStatus.Created;
      component['relationData'] = new Array(relationData);
      component['handleDeleteRelationEvent'](relationData);

      expect(spyOnDeleteSoftSavedRelation).toHaveBeenCalled();
    });
  });
});
