import { TestBed, inject } from '@angular/core/testing';
import { FieldService } from './field.service';
import { EditPanelFacade } from './editpanelfield.facade';
import { of } from 'rxjs/internal/observable/of';
import { Store } from '@ngrx/store';
import { HttpClient, HttpHandler } from '@angular/common/http';
import { Field } from '../models/field.model';
import { Observable, throwError } from 'rxjs';
import * as fromFieldState from '../+state/reducers/field.reducers';
import * as fieldActions from '../+state/actions/field.actions';

const fieldObject = (fieldID: string) => {
  return {
    fieldID,
    data: ['2019-07-15T22:00:00.0000000Z', '2019-07-17T22:00:00.0000000Z', '2019-07-16T22:00:00.0000000Z'],
  } as Field;
};

const updateField = 'updateField';

const emptyEntityresponse = {
  entities: {},
  ids: [],
  subscribe() {
    return fieldObject('old');
  },
};

const storeMock = {
  select() {
    return of(emptyEntityresponse);
  },
  dispatch() {
    return jest.fn();
  },
};

class FieldServiceMock {
  public updateFieldValues(field: Field): Observable<Field> {
    return field.fieldID === 'success' ? of(field) : throwError('Error');
  }
}

describe('EditPanel Facade', () => {
  let service: EditPanelFacade;
  let fieldStore: Store<fromFieldState.FeatureState>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: FieldService,
          useClass: FieldServiceMock,
        },
        HttpClient,
        HttpHandler,
        {
          provide: Store,
          useValue: storeMock,
        },
        EditPanelFacade,
      ],
    }).compileComponents();
    service = TestBed.get(EditPanelFacade);
    fieldStore = TestBed.get(Store);
  });
  afterEach(() => {
    service.ngOnDestroy();
    service = null;
    fieldStore = null;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('updateFieldValues() should', () => {
    it('send a action Update fields', async () => {
      const field = fieldObject('success');

      const spyOnSave = spyOn<any>(fieldStore, 'dispatch');

      await service.updateFieldValues(field);

      expect(spyOnSave).toHaveBeenCalledWith(new fieldActions.UpdateField(field));
    });
    it('send a action Update success fields', inject([FieldService], async () => {
      const fieldNew = fieldObject('success');
      const fieldOld = fieldObject('old');

      const spyOnValue = spyOn<any>(fieldStore, 'dispatch');

      service[updateField](fieldNew, fieldOld);

      expect(spyOnValue).toHaveBeenCalledWith(new fieldActions.UpdateFieldSuccess());
    }));
    it('send a action Update Faild fields', inject([FieldService], async () => {
      const fieldNew = fieldObject('failed');
      const fieldOld = fieldObject('old');

      const spyOnValue = spyOn<any>(fieldStore, 'dispatch');

      service[updateField](fieldNew, fieldOld);

      expect(spyOnValue).toHaveBeenCalledWith(new fieldActions.UpdateFieldFail(fieldOld));
    }));
  });
});
