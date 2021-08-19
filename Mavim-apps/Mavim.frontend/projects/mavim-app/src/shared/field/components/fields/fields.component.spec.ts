/* eslint-disable dot-notation, @typescript-eslint/dot-notation */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FieldsComponent } from './fields.component';
import { NO_ERRORS_SCHEMA } from '@angular/compiler/src/core';
import { DevExtremeModule } from 'devextreme-angular';
import { of } from 'rxjs';
import { Store } from '@ngrx/store';

describe('FieldsComponent', () => {
  let component: FieldsComponent;
  let fixture: ComponentFixture<FieldsComponent>;

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
      declarations: [FieldsComponent],
      imports: [DevExtremeModule],
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
    fixture = TestBed.createComponent(FieldsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('emitInternalLinkClickEvent() should emit a dcv', () => {
    const spy = jest.spyOn(component['internalDcvId'], 'emit');
    const dcv = 'somedcv';

    component.emitInternalLinkEvent(dcv);

    expect(spy).toHaveBeenCalledWith(dcv);
  });

  it('renderCellValueContent() should return match when the field has link value', () => {
    const fieldValue = 'True';
    const expected = 'Yes';

    const actual = component.renderCellValueContent(fieldValue, 'Boolean');

    expect(actual).toEqual(expected);
  });

  it('renderCellValueContent() should return match when the field has link value', () => {
    const fieldValue = 'False';
    const expected = 'No';

    const actual = component.renderCellValueContent(fieldValue, 'Boolean');

    expect(actual).toEqual(expected);
  });

  it('renderCellValueContent() should return dutch formatted date based on isoString', () => {
    const expected = '1 januari 2010'; // default is dutch.
    const isoDate = new Date('01-01-2010').toISOString();
    // eslint-disable-next-line no-null/no-null
    const nullDate = null; // It can be nullable datetime from the API.

    expect(component['dateFormat'](isoDate)).toEqual(expected);
    expect(component['dateFormat'](nullDate)).toEqual('');
  });

  it('renderCellValueContent() should return match when the field is of type list', () => {
    const fieldValue = 'keyValue:DisplayValue';
    const expected = 'DisplayValue';

    const actual = component.renderCellValueContent(fieldValue, 'List');

    expect(actual).toEqual(expected);
  });
});
