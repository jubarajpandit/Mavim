import { TestBed } from '@angular/core/testing';
import { RelationService } from './relation.service';
import { HttpClient, HttpHandler } from '@angular/common/http';
import { ErrorService } from '../../shared/services/error.service';

describe('RelationService', () => {
  let service: RelationService;
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
        RelationService,
        HttpClient,
        HttpHandler,
        {
          provide: ErrorService,
          useValue: errorServiceMock,
        },
      ],
    });

    service = TestBed.get(RelationService);
    httpClient = TestBed.get(HttpClient);
    errorService = TestBed.get(ErrorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call httpClient get method', () => {
    const spy = jest.spyOn(httpClient, 'get');

    service.getRelations('somedcv');

    expect(spy).toHaveBeenCalled();
  });
});
