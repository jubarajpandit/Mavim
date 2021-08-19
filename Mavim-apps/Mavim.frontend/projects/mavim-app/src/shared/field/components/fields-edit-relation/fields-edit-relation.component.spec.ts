import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { TreeModule } from '../../../tree/tree.module';
import { ControlsModule } from '../../../controls/controls.module';
import { FieldsEditRelationComponent } from './fields-edit-relation.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';

describe('FieldsEditRelationComponent', () => {
  let component: FieldsEditRelationComponent;
  let fixture: ComponentFixture<FieldsEditRelationComponent>;

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
      declarations: [FieldsEditRelationComponent],
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
    fixture = TestBed.createComponent(FieldsEditRelationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
