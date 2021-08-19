import { Dictionary } from '@ngrx/entity';
import { createAction, props } from '@ngrx/store';
import { TopicMetaType } from '../../models/topic-meta-type.model';

export const LoadTopicTypes = createAction(
	'[TopicMeta] Load Topic Types',
	props<{ payload: string }>()
);
export const LoadTopicTypesSuccess = createAction(
	'[TopicMeta] Load Topic Types Success',
	props<{ payload: Dictionary<TopicMetaType> }>()
);
export const LoadTopicTypesFailed = createAction(
	'[TopicMeta] Load Topic Types Failed'
);
export const LoadTopicIcons = createAction(
	'[TopicMeta] Load Topic Icons',
	props<{ payload: string }>()
);
export const LoadTopicIconsSuccess = createAction(
	'[TopicMeta] Load Topic Icons Success',
	props<{ payload: Dictionary<string> }>()
);
export const LoadTopicIconsFailed = createAction(
	'[TopicMeta] Load Topic Icons Failed'
);
export const ClearTopicMetaStore = createAction('[TopicMeta] Clear Store');
