import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DynamicPanelComponent } from './dynamic-panel.component';
import { Store } from '@ngrx/store';
import { MockStore } from '@ngrx/store/testing';
import { of, Subject, Observable } from 'rxjs';
import { NO_ERRORS_SCHEMA } from '@angular/compiler/src/core';
import { RouterTestingModule } from '@angular/router/testing';
import { FieldsComponent } from '../../../field/components/fields/fields.component';
import { RelationsComponent } from '../../../relation/components/relations/relations.component';
import { FilesComponent } from '../../../file/components/files/files.component';
import { DevExtremeModule } from 'devextreme-angular';
import { DotLoaderComponent } from '../../../loaders/dot-loader/dot-loader.component';

describe('DynamicPanelComponent', () => {
  let component: DynamicPanelComponent;
  let fixture: ComponentFixture<DynamicPanelComponent>;
  let store: MockStore<{}>;
  let topicStore: MockStore<{ topics: []; splitScreen: { fullEditState: 'edit' } }>;
  let notificationStore: MockStore<{ notifications: [] }>;
  let fieldStore: MockStore<{ fields: [] }>;
  let fileStore: MockStore<{ files: [] }>;
  let relationStore: MockStore<{ relations: [] }>;
  let subtopicsStore: MockStore<{ childTopics: [] }>;
  const unsubscribe = new Subject<void>();
  const emptyEntityresponse = {
    entities: {},
    ids: [],
    subscribe() {
      return true;
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

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DynamicPanelComponent, DotLoaderComponent, FieldsComponent, RelationsComponent, FilesComponent],
      imports: [RouterTestingModule, DevExtremeModule],
      providers: [
        {
          provide: Store,
          useValue: storeMock,
        },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DynamicPanelComponent);
    component = fixture.componentInstance;
    store = TestBed.get(Store);
    topicStore = TestBed.get(Store);
    fileStore = TestBed.get(Store);
    fieldStore = TestBed.get(Store);
    relationStore = TestBed.get(Store);
    notificationStore = TestBed.get(Store);
    subtopicsStore = TestBed.get(Store);
    fixture.detectChanges();
  });

  afterEach(() => {
    unsubscribe.next();
    unsubscribe.complete();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
