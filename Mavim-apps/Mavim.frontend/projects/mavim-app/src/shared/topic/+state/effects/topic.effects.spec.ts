import { Observable, empty } from 'rxjs';
import { TopicEffects } from './topic.effects';
import { TopicService } from '../../services/topic.service';
import { TestBed } from '@angular/core/testing';
import { hot, cold } from 'jasmine-marbles';
import { provideMockActions } from '@ngrx/effects/testing';
import { LoadTopicSuccess, LoadTopicRoot } from '../actions/topic.actions';
import { Topic } from '../../models/topic.model';
import { Actions } from '@ngrx/effects';
import { TreeService } from '../../../tree/services/tree.service';

export class TestActions extends Actions {
  public constructor() {
    super(empty());
  }
}

export function getActions() {
  return new TestActions();
}

describe('TopicEffects', () => {
  let actions: TestActions;
  let effects: TopicEffects;
  let topicService: TopicService;
  let treeService: TreeService;

  const treeServiceMock = {
    flattenHierarchicalTreeData: jest.fn(),
  };

  const topicServiceMock = {
    getTree: jest.fn(),
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        TopicEffects,
        provideMockActions(() => actions),
        {
          provide: TopicService,
          useValue: topicServiceMock,
        },
        {
          provide: TreeService,
          useValue: treeServiceMock,
        },
      ],
    }).compileComponents();

    effects = TestBed.get(TopicEffects);
    topicService = TestBed.get(TopicService);
    treeService = TestBed.get(TreeService);
  });

  it('should be created', () => {
    expect(effects).toBeTruthy();
  });

  describe('loadTopics', () => {
    const action = new LoadTopicRoot();

    it('should return a LoadSuccess action, with the topics, on success', () => {
      const topics: Topic = {
        dcv: 'string',
        parent: 'string',
        name: 'string',
        status: 'string',
        typeCategory: 'string',
        hasChildren: true,
        icon: 'string',
      } as Topic;
      const outcome = new LoadTopicSuccess(topics);

      actions = hot('-a|', { a: action });
      const response = cold('-a', { a: topics });
      const expected = cold('--b', { b: outcome });
      topicService.getTreeRoot = jest.fn(() => response);

      expect(effects.loadTree$).toBeObservable(expected);
    });
  });
});
