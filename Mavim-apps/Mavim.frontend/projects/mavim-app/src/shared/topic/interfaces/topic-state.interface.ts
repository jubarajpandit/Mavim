import { Topic } from '../models/topic.model';
import { EntityState } from '@ngrx/entity';

export interface TopicState extends EntityState<Topic> {
	allTopicsLoaded: boolean;
	lastTopicUpdated: Topic;
	topicLoaded: boolean;
}
