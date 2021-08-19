import { createAction, props } from '@ngrx/store';
import { TopicResource } from '../../enums/topic-resource.enum';
import { Topic } from '../../models/topic.model';

export const CreateTopic = createAction(
	'[Topic] Create Topic',
	props<{
		payload: { topicId: string; name: string; type: string; icon: string };
	}>()
);
export const CreateTopicSuccess = createAction(
	'[Topic] Create Topic Success',
	props<{ payload: { topic: Topic; topicAbove: string } }>()
);
export const CreateTopicFailure = createAction('[Topic] Create Topic Failed');

export const CreateChildTopic = createAction(
	'[Topic] Create Child Topic',
	props<{
		payload: {
			parentTopicId: string;
			name: string;
			type: string;
			icon: string;
		};
	}>()
);
export const CreateChildTopicSuccess = createAction(
	'[Topic] Create Child Topic Success',
	props<{ payload: { parentTopicId: string; topic: Topic } }>()
);
export const CreateChildTopicFailure = createAction(
	'[Topic] Create Child Topic Failed',
	props<{ payload: string }>()
);

export const DeleteTopic = createAction(
	'[Topic] Delete Topic',
	props<{ payload: { topicId: string; recycleBinId: string } }>()
);
export const DeleteTopicSuccess = createAction(
	'[Topic] Delete Topic Success',
	props<{ payload: { topicId: string; recycleBinId: string } }>()
);
export const DeleteTopicFailure = createAction(
	'[Topic] Delete Topic Failed',
	props<{ payload: string }>()
);

export const LoadTopicRoot = createAction('[Topic] Load Topic Root');
export const LoadTopicRootSuccess = createAction(
	'[Topic] Load Root Topic Success',
	props<{ payload: Topic }>()
);

export const LoadTopicChildren = createAction(
	'[Topic] Load Topic Children',
	props<{ payload: string }>()
);
export const LoadTopicChildrenSuccess = createAction(
	'[Topic] Load Children Topics Success',
	props<{ payload: Topic[] }>()
);
export const LoadTopicSiblingsSuccess = createAction(
	'[Topic] Load Siblings Topics Success',
	props<{ payload: Topic[] }>()
);

export const LoadRelationshipTypes = createAction(
	'[Topic] Load Relationship Type elements'
);
export const LoadRelationshipSuccess = createAction(
	'[Topic] Load Relationship Topics Success',
	props<{ payload: Topic[] }>()
);

export const LoadTopicFail = createAction('[Topic] Load Topic Fail');
export const LoadTopicFailWithStatusCode = createAction(
	'[Topic] Load Topic Fail With Status Code',
	props<{ payload: Topic }>()
);
export const LoadTopicSuccess = createAction(
	'[Topic] Load Topic Success',
	props<{ payload: Topic }>()
);
export const LoadTopicByDCV = createAction(
	'[Topic] Load topic by Dcv',
	props<{ payload: string }>()
);

export const UpdateTopicName = createAction(
	'[Topic] Update Topic Name',
	props<{ payload: Topic }>()
);
export const UpdateTopicNameSuccess = createAction(
	'[Topic] Update Topic Name Success',
	props<{ payload: Topic }>()
);
export const AddTopicResource = createAction(
	'[Topic] Add Topic Resource',
	props<{ payload: Topic }>()
);
export const RemoveTopicResource = createAction(
	'[Topic] Remove Topic Resource',
	props<{ payload: { resource: TopicResource; topicId: string } }>()
);
