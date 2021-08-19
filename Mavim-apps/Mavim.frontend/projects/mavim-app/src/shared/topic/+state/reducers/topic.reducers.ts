import {
	EntityAdapter,
	createEntityAdapter,
	Dictionary,
	Update
} from '@ngrx/entity';
import * as TopicActions from '../actions/topic.actions';
import * as LanguageActions from '../../../language/+state/language.actions';
import * as RelationActions from '../../../relation/+state/actions/relation.actions';
import { TopicState } from '../../interfaces/topic-state.interface';
import { Topic } from '../../models/topic.model';
import * as TreeActions from '../../../tree/+state';
import { createReducer, on } from '@ngrx/store';
import { TopicResource } from '../../enums/topic-resource.enum';
import * as RouterActions from '../../../router/+state/routing.actions';

export const topicsAdapter: EntityAdapter<Topic> = createEntityAdapter<Topic>({
	selectId: (topic) => topic.dcv
});

export const initialTopicState: TopicState = topicsAdapter.getInitialState({
	topicLoaded: true,
	allTopicsLoaded: true,
	lastTopicUpdated: undefined
});

export const topicReducer = createReducer(
	initialTopicState,
	on(
		TopicActions.LoadTopicRoot,
		(state): TopicState => ({
			...state,
			topicLoaded: false
		})
	),
	on(
		TopicActions.LoadRelationshipTypes,
		TopicActions.LoadTopicChildren,
		(state): TopicState => ({
			...state,
			allTopicsLoaded: false
		})
	),
	on(TopicActions.CreateChildTopicSuccess, (state, { payload }) =>
		topicsAdapter.upsertMany(
			createNewChildTopic(
				state.entities,
				payload.topic,
				payload.parentTopicId
			),
			{ ...state }
		)
	),
	on(
		TopicActions.LoadTopicChildrenSuccess,
		TopicActions.LoadTopicSiblingsSuccess,
		TopicActions.LoadRelationshipSuccess,
		(state, { payload }) =>
			topicsAdapter.addMany(payload, { ...state, allTopicsLoaded: true })
	),
	on(
		TopicActions.LoadTopicFail,
		(state): TopicState => ({
			...state,
			topicLoaded: true,
			allTopicsLoaded: true
		})
	),
	on(TopicActions.LoadTopicFailWithStatusCode, (state, { payload }) =>
		topicsAdapter.upsertOne(payload, {
			...state,
			topicLoaded: true,
			allTopicsLoaded: true
		})
	),
	on(
		TopicActions.UpdateTopicName,
		(state): TopicState => ({
			...state,
			topicLoaded: false
		})
	),
	on(TopicActions.UpdateTopicNameSuccess, (state, { payload }) => {
		const updatedTopic = {
			id: payload.dcv,
			changes: {
				name: payload.name
			}
		} as Update<Topic>;
		return topicsAdapter.updateOne(updatedTopic, {
			...state,
			lastTopicUpdated: payload,
			topicLoaded: true
		});
	}),
	on(
		TopicActions.LoadTopicSuccess,
		TopicActions.LoadTopicRootSuccess,
		(state, { payload }) =>
			topicsAdapter.upsertOne(payload, { ...state, topicLoaded: true })
	),
	on(TopicActions.CreateTopicSuccess, (state, { payload }) =>
		topicsAdapter.upsertMany(
			createNewTopic(state.entities, payload.topic),
			{ ...state }
		)
	),
	on(TopicActions.DeleteTopicSuccess, (state, { payload }) =>
		deleteTopic(state, payload.topicId, payload.recycleBinId)
	),

	on(TreeActions.ExpandToNodeSuccess, (state, { payload }) =>
		topicsAdapter.addMany(payload.path.data, { ...state })
	),
	on(
		TreeActions.MoveToTop,
		TreeActions.MoveToBottom,
		TreeActions.MoveUp,
		TreeActions.MoveDown,
		TreeActions.MoveLevelUp,
		TreeActions.MoveLevelDown,
		(state): TopicState => ({ ...state, allTopicsLoaded: false })
	),
	on(TreeActions.MoveToTopSuccess, (state, { payload }) =>
		topicsAdapter.updateMany(moveToTop(state.entities, payload.topicId), {
			...state,
			allTopicsLoaded: true
		})
	),
	on(TreeActions.MoveToBottomSuccess, (state, { payload }) =>
		topicsAdapter.updateMany(
			moveToBottom(state.entities, payload.topicId),
			{
				...state,
				allTopicsLoaded: true
			}
		)
	),
	on(TreeActions.MoveUpSuccess, (state, { payload }) =>
		topicsAdapter.updateMany(moveUp(state.entities, payload.topicId), {
			...state,
			allTopicsLoaded: true
		})
	),
	on(TreeActions.MoveDownSuccess, (state, { payload }) =>
		topicsAdapter.updateMany(moveDown(state.entities, payload.topicId), {
			...state,
			allTopicsLoaded: true
		})
	),
	on(TreeActions.MoveLevelUpSuccess, (state, { payload }) =>
		topicsAdapter.updateMany(moveLevelUp(state.entities, payload.topicId), {
			...state,
			allTopicsLoaded: true
		})
	),
	on(TreeActions.MoveLevelDownSuccess, (state, { payload }) =>
		topicsAdapter.updateMany(
			moveLevelDown(state.entities, payload.topicId),
			{
				...state,
				allTopicsLoaded: true
			}
		)
	),
	on(
		TreeActions.MoveFailed,
		(state): TopicState => ({
			...state,
			allTopicsLoaded: true
		})
	),
	on(
		LanguageActions.UpdateLanguage,
		(): TopicState => ({ ...initialTopicState })
	),
	on(RelationActions.CreateRelationSuccess, (state, { payload }) => {
		const updatedTopic = addResourceIfNotExists(
			state,
			payload.topicDCV,
			TopicResource.Relations
		);
		return updatedTopic
			? topicsAdapter.updateOne(updatedTopic, state)
			: state;
	}),
	on(RouterActions.CreateNewWord, (state, { payload }) => {
		const updatedTopic = addResourceIfNotExists(
			state,
			payload.dcv,
			TopicResource.Description
		);
		return updatedTopic
			? topicsAdapter.updateOne(updatedTopic, state)
			: state;
	})
);

const deleteTopic = (
	state: TopicState,
	topicId: string,
	recycleBinId: string
): TopicState => {
	const deletedTopic = state.entities[topicId];
	const topicList = getAllSubTopics(state, deletedTopic.dcv, []);
	const recycleBinChildCount = Object.values(state.entities).filter(
		(topic) => topic.parent === recycleBinId
	).length;
	if (!deletedTopic?.isInRecycleBin && recycleBinChildCount > 0) {
		const subTopicsChanges = topicList.map((topic) => {
			return {
				id: topic.dcv,
				changes: {
					isInRecycleBin: true
				}
			} as Update<Topic>;
		});

		const deletedTopicChange = {
			id: topicId,
			changes: {
				parent: recycleBinId,
				isInRecycleBin: true,
				business: {
					...deletedTopic.business,
					canCreateChildTopic: false,
					canCreateTopicAfter: false
				}
			}
		} as Update<Topic>;

		return topicsAdapter.updateMany(
			[...subTopicsChanges, deletedTopicChange],
			{ ...state }
		);
	}

	return topicsAdapter.removeMany(
		[...topicList, deletedTopic].map((topic) => topic.dcv),
		{ ...state }
	);
};

const getAllSubTopics = (
	state: TopicState,
	parentId: string,
	topics: Topic[]
): Topic[] => {
	const subTopics = Object.values(state.entities).filter(
		(topic) => topic.parent === parentId
	);
	topics.push(...subTopics);

	subTopics.forEach((topic) => {
		getAllSubTopics(state, topic.dcv, topics);
	});

	return topics;
};

const moveToTop = (
	entities: Dictionary<Topic>,
	topicId: string
): Update<Topic>[] => {
	const siblingsAbove = getSiblingsAbove(entities, topicId);
	const orderNumbers = siblingsAbove.map((topic) => {
		return topic.orderNumber;
	});
	const minOrderNumber = Math.min(...orderNumbers);

	return siblingsAbove.map((topic) => {
		return {
			id: topic.dcv,
			changes: {
				orderNumber:
					topic === entities[topicId]
						? minOrderNumber
						: topic.orderNumber + 1
			}
		} as Update<Topic>;
	});
};

const moveUp = (
	entities: Dictionary<Topic>,
	topicId: string
): Update<Topic>[] => {
	const lastTwoElements = -2;
	const baseTopic = entities[topicId];
	const previousSibling = getSiblingsAbove(entities, topicId)
		.sort((a, b) => a.orderNumber - b.orderNumber)
		.slice(lastTwoElements)[0];

	return [
		{
			id: baseTopic.dcv,
			changes: {
				orderNumber: previousSibling.orderNumber
			}
		},
		{
			id: previousSibling.dcv,
			changes: {
				orderNumber: baseTopic.orderNumber
			}
		}
	] as Update<Topic>[];
};

const moveDown = (
	entities: Dictionary<Topic>,
	topicId: string
): Update<Topic>[] => {
	const secondElement = 1;
	const baseTopic = entities[topicId];
	const toTopic = getSiblingsBelow(entities, topicId).sort(
		(a, b) => a.orderNumber - b.orderNumber
	)[secondElement];

	return [
		{
			id: baseTopic.dcv,
			changes: {
				orderNumber: toTopic.orderNumber
			}
		},
		{
			id: toTopic.dcv,
			changes: {
				orderNumber: baseTopic.orderNumber
			}
		}
	] as Update<Topic>[];
};

const createNewTopic = (entities: Dictionary<Topic>, topic: Topic): Topic[] => {
	const topicEntities: Topic[] = Object.keys(entities)
		.map((key) => ({ ...entities[key] }))
		.filter((t) => t.parent === topic.parent);

	topicEntities.forEach((entity) => {
		if (entity.orderNumber >= topic.orderNumber) {
			entity.orderNumber++;
		}
	});

	topicEntities.push(topic);

	return topicEntities;
};

const createNewChildTopic = (
	entities: Dictionary<Topic>,
	child: Topic,
	parentId: string
): Topic[] => {
	const parent: Topic = { ...entities[parentId] };
	parent.hasChildren = true;

	return [parent, child];
};

const moveLevelUp = (
	entities: Dictionary<Topic>,
	topicId: string
): Update<Topic>[] => {
	const topicToMove: Topic = entities[topicId];
	const parent: Topic = entities[topicToMove.parent];
	const children = Object.values(entities).filter(
		(topic: Topic) => topic.parent === parent.dcv
	);

	const topicToMoveChange = {
		id: topicToMove.dcv,
		changes: {
			parent: parent.parent,
			orderNumber: parent.orderNumber + 1
		}
	} as Update<Topic>;

	const siblingsBelow = getSiblingsBelow(entities, parent.dcv);
	const topicsBelowParentChanges = siblingsBelow.map((topic) => {
		return {
			id: topic.dcv,
			changes: {
				orderNumber:
					topic === entities[parent.dcv]
						? topic.orderNumber
						: topic.orderNumber + 1
			}
		} as Update<Topic>;
	});

	if (children.length === 1) {
		topicsBelowParentChanges.push({
			id: parent.dcv,
			changes: {
				hasChildren: false
			}
		} as Update<Topic>);
	}

	return topicsBelowParentChanges.concat(topicToMoveChange);
};

const moveLevelDown = (
	entities: Dictionary<Topic>,
	topicId: string
): Update<Topic>[] => {
	const lastTwoElements = -2;
	const topicToMove = entities[topicId];
	const previousSibling = getSiblingsAbove(entities, topicId)
		.sort((a, b) => a.orderNumber - b.orderNumber)
		.slice(lastTwoElements)[0];
	const children = Object.values(entities).filter(
		(topic: Topic) => topic.parent === previousSibling.dcv
	);
	const orderNumbers = children.map((topic) => {
		return topic.orderNumber;
	});
	const maxOrderNumber = Math.max(...orderNumbers);

	const siblingsBelow = getSiblingsBelow(entities, topicToMove.dcv);
	const topicsBelowTopicToMoveChanges = siblingsBelow.map((topic) => {
		return {
			id: topic.dcv,
			changes: {
				orderNumber:
					topic === entities[topicToMove.dcv]
						? topic.orderNumber
						: topic.orderNumber - 1
			}
		} as Update<Topic>;
	});

	const previousSiblingChange = {
		id: previousSibling.dcv,
		changes: {
			hasChildren: true
		}
	};

	const topicToMoveChange = {
		id: topicToMove.dcv,
		changes: {
			parent: previousSibling.dcv,
			orderNumber: children.length > 0 ? maxOrderNumber + 1 : 0
		}
	} as Update<Topic>;

	return topicsBelowTopicToMoveChanges.concat([
		previousSiblingChange,
		topicToMoveChange
	]);
};

const getSiblingsAbove = (
	entities: Dictionary<Topic>,
	topicId: string
): Topic[] => {
	return Object.values(entities).filter(
		(topic) =>
			topic.parent === entities[topicId].parent &&
			topic.orderNumber <= entities[topicId].orderNumber
	);
};

const getSiblingsBelow = (
	entities: Dictionary<Topic>,
	topicId: string
): Topic[] => {
	return Object.values(entities).filter(
		(topic) =>
			topic.parent === entities[topicId].parent &&
			topic.orderNumber >= entities[topicId].orderNumber
	);
};

const moveToBottom = (
	entities: Dictionary<Topic>,
	topicId: string
): Update<Topic>[] => {
	const siblingsBelow = getSiblingsBelow(entities, topicId);
	const orderNumbers = siblingsBelow.map((topic) => {
		return topic.orderNumber;
	});
	const maxOrderNumber = Math.max(...orderNumbers);

	return siblingsBelow.map((topic) => {
		return {
			id: topic.dcv,
			changes: {
				orderNumber:
					topic === entities[topicId]
						? maxOrderNumber
						: topic.orderNumber - 1
			}
		} as Update<Topic>;
	});
};

const addResourceIfNotExists = (
	state: TopicState,
	topicId: string,
	resource: TopicResource
): Update<Topic> => {
	const topic = state.entities[topicId];
	const isResourceExists = topic?.resources?.find((r) => r === resource);
	if (isResourceExists) {
		return;
	}
	const resources = [...topic.resources, resource];
	return {
		id: topicId,
		changes: {
			resources
		}
	} as Update<Topic>;
};
