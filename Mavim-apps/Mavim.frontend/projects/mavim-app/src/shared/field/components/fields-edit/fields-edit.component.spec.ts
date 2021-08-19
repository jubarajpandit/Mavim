import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FieldsEditComponent } from './fields-edit.component';
import { DevExtremeModule } from 'devextreme-angular';
import { Field } from '../../models/field.model';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { EditField } from '../../models/edit-field.model';
import { EditStatus } from '../../../shared/enums/edit-status.enum';

describe('FieldsComponent', () => {
  let component: FieldsEditComponent;
  let fixture: ComponentFixture<FieldsEditComponent>;

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
      declarations: [FieldsEditComponent],
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
    fixture = TestBed.createComponent(FieldsEditComponent);
    component = fixture.componentInstance;
    component.ngOnInit = jest.fn();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('isEmpty should show different behaviour on different strings', () => {
    const undefinedString = undefined;
    const filledString = 'test';
    const emptyString = '';

    expect((component as any).isEmpty(undefinedString)).toEqual(false);
    expect((component as any).isEmpty(filledString)).toEqual(false);
    expect((component as any).isEmpty(emptyString)).toEqual(true);
  });

  it('isDecimal should show different behaviour on different decimals', () => {
    const normalDecimal = '4,2';
    const wholeNumberDecimal = '4';
    const dotDecimal = '0.2';
    const twoDotsInDecimal = '4,2,3';
    const decimalWithAlphaChar = '4,A,3';

    expect((component as any).isDecimal(normalDecimal)).toEqual(true);
    expect((component as any).isDecimal(wholeNumberDecimal)).toEqual(true);
    expect((component as any).isDecimal(dotDecimal)).toEqual(false);
    expect((component as any).isDecimal(twoDotsInDecimal)).toEqual(false);
    expect((component as any).isDecimal(decimalWithAlphaChar)).toEqual(false);
  });

  it('onDateChangeEvent() should emit updated Fields', () => {
    const changeEvent = {
      valueAsDate: new Date('2019-07-01'),
      name: '2',
    } as HTMLInputElement;

    const changeField = {
      fieldID: 'd5926266c6378v0',
      data: ['2019-07-15T22:00:00.0000000Z', '2019-07-17T22:00:00.0000000Z', '2019-07-16T22:00:00.0000000Z'],
    } as EditField;

    const spy = spyOn(component.fieldsChanged, 'emit');

    component.changedFields = [changeField];

    component.onDateChangeEvent(changeField, changeEvent);

    expect(spy).toHaveBeenCalledWith([changeField]);
  });

  it('onListValueChanged() should emit updated list value', () => {
    const changeEvent = {
      value: 'd5926266c1477v0',
      name: '2',
    } as HTMLInputElement;

    const changeField = {
      ...new EditField(),
      fieldID: 'd5926266c6378v0',
      data: ['d5926266c1476v0', 'd5926266c1477v0', 'd5926266c1479v0'],
      options: {
        d5926266c1476v0: '2 Example Field Set (empty)',
        d5926266c1477v0: '1 Example Field Set (filled)',
        d5926266c1479v0: '2 Example Field Set (empty)',
        d5926266c1489v0: 'test',
        d5926266c3017v0: '3 Field example with fieldrelations',
        d5926266c3018v0: 'Target of fieldrelations',
        d5926266c6430v0: 'Boolean Sample',
        d5926266c6439v0: 'test',
      },
    } as EditField;

    const index = 0;
    const spy = spyOn(component.fieldsChanged, 'emit');

    component.changedFields = [changeField];

    component.onListValueChanged(changeField, changeEvent, index);

    expect(spy).toHaveBeenCalledWith([changeField]);
  });

  describe('showModal should be', () => {
    it('true when onRelationChangedEvent() is executed', () => {
      component.onRelationChangedEvent(undefined, undefined);

      expect(component.showModal).toEqual(true);
    });

    it('false when hideFieldRelationsModal() is executed', () => {
      component.hideFieldRelationsModal();

      expect(component.showModal).toEqual(false);
    });
  });
});
