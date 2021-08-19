import { TestBed } from '@angular/core/testing';
import { TopicService } from './topic.service';
import { HttpClient, HttpHandler } from '@angular/common/http';
import { Topic } from '../models/topic.model';
import { ErrorService } from '../../shared/services/error.service';

describe('TopicService', () => {
  let service: TopicService;
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
        TopicService,
        HttpClient,
        HttpHandler,
        {
          provide: ErrorService,
          useValue: errorServiceMock,
        },
      ],
    }).compileComponents();

    service = TestBed.get(TopicService);
    httpClient = TestBed.get(HttpClient);
    errorService = TestBed.get(ErrorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getTreeRoot() should call httpClient get method', () => {
    const spy = jest.spyOn(httpClient, 'get');

    service.getTreeRoot();

    expect(spy).toHaveBeenCalled();
  });

  it('getTreeChildElements(dcv) should call httpClient getMethode', () => {
    const spy = jest.spyOn(httpClient, 'get');
    const dcv = 'testDcv';

    service.getTreeChildElements(dcv);

    expect(spy).toHaveBeenCalled();
  });

  it('should call httpClient patch method', () => {
    const spy = jest.spyOn(httpClient, 'patch');
    const topic = new Topic();

    service.updateTopicName(topic);

    expect(spy).toHaveBeenCalled();
  });
});
