import { TestBed } from '@angular/core/testing';
import { FieldService } from './field.service';
import { HttpClient, HttpHandler } from '@angular/common/http';
import { ErrorService } from '../../shared/services/error.service';
import { Field } from '../models/field.model';

describe('FileService', () => {
  let service: FieldService;
  let httpClient: HttpClient;
  let errorService: ErrorService;

  const errorServiceMock = {
    handleServiceError(error, type) {
      return jest.fn();
    },
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        FieldService,
        HttpClient,
        HttpHandler,
        {
          provide: ErrorService,
          useValue: errorServiceMock,
        },
      ],
    }).compileComponents();

    service = TestBed.get(FieldService);
    httpClient = TestBed.get(HttpClient);
    errorService = TestBed.get(ErrorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getFieldSets() should call httpClient get method', () => {
    const spy = jest.spyOn(httpClient, 'get');

    service.getFieldSets('somedcv');

    expect(spy).toHaveBeenCalled();
  });

  it('updateFieldValues() should call httpClient patch method', () => {
    const spy = jest.spyOn(httpClient, 'patch');
    const field: Field = new Field();
    field.data = ['someFieldValue'];
    field.topicDCV = 'somedcv';
    field.fieldSetID = 'fieldsetId';
    field.fieldID = 'fieldset';

    service.updateFieldValues(field);

    expect(spy).toHaveBeenCalled();
  });
});
