import { TreeState } from '../interfaces/tree-state.interface';
import * as TopicActions from '../../topic/+state/actions/topic.actions';
import * as TreeEditActions from './tree-edit.actions';
import { FlatTreeNode } from '../models/flat-tree-node.model';
import { Path } from '../models/api/path';
import { Dictionary } from '@ngrx/entity';
import { Topic } from '../../topic/models/topic.model';
import * as TreeActions from './generictree.actions';
import { createReducer, on } from '@ngrx/store';
import { FetchStatus } from '../../enums/FetchState';
import { ActionPayloadWithNameSpace } from '../interfaces/action-payload-with-namespace';

export const initialTreeState: TreeState = {
	fetchStatus: FetchStatus.NotFetched,
	selectedNode: undefined,
	nodes: []
};

export function createTreeReducerWithNameData(
	namespace: string
): (state: TreeState, action: ActionPayloadWithNameSpace) => TreeState {
	return function treeReducerValidator(
		state = initialTreeState,
		action: ActionPayloadWithNameSpace
	): TreeState {
		const { namespace: actionNameSpace } = action?.payload;

		if (actionNameSpace !== namespace) {
			return state;
		}
		return treeReducerWithNameData(state, action);
	};
}

export const topicTreeReducer = createReducer(
	initialTreeState,
	on(TopicActions.UpdateTopicNameSuccess, (state, action) => ({
		...state,
		nodes: updateNodeName(
			state.nodes,
			action.payload.dcv,
			action.payload.name
		)
	})),
	on(TopicActions.CreateTopicSuccess, (state, action) => ({
		...state,
		nodes: addNodeUnder(
			state.nodes,
			action.payload.topic,
			action.payload.topicAbove
		)
	})),
	on(TopicActions.CreateChildTopic, (state, action) => ({
		...state,
		nodes: toggleNodeLoading(state.nodes, action.payload.parentTopicId)
	})),
	on(TopicActions.CreateChildTopicSuccess, (state, action) => ({
		...state,
		nodes: createChildNode(
			state.nodes,
			action.payload.topic,
			action.payload.parentTopicId
		)
	})),
	on(TopicActions.DeleteTopic, (state, action) => ({
		...state,
		nodes: toggleNodeLoading(state.nodes, action.payload.topicId)
	})),
	on(TopicActions.DeleteTopicFailure, (state, action) => ({
		...state,
		nodes: toggleNodeLoading(state.nodes, action.payload)
	})),
	on(TopicActions.DeleteTopicSuccess, (state, action) => ({
		...state,
		nodes: deleteNode(
			state.nodes,
			action.payload.topicId,
			action.payload.recycleBinId
		)
	}))
);

export const treeReducerWithNameData = createReducer(
	initialTreeState,
	on(TreeActions.ClearTree, (): TreeState => ({ ...initialTreeState })),
	on(
		TreeActions.ExpandToNode,
		(): TreeState => ({
			...initialTreeState,
			fetchStatus: FetchStatus.Loading
		})
	),
	on(TreeActions.ExpandToNodeSuccess, (state, action) => ({
		...state,
		nodes: mapPath(action.payload.path),
		fetchStatus: FetchStatus.Fetched
	})),
	on(
		TreeActions.ExpandToNodeFailed,
		(state): TreeState => ({
			...state,
			fetchStatus: FetchStatus.Failed
		})
	),
	on(TreeActions.AddChildNodes, (state, action) => ({
		...state,
		nodes: addChildNodes(
			state.nodes,
			action.payload.children,
			action.payload.parent.dcvId
		)
	})),
	on(TreeActions.RemoveChildNodes, (state, action) => ({
		...state,
		nodes: removeChildNodes(
			state.nodes,
			action.payload.node.level,
			action.payload.node.dcvId
		)
	})),
	on(TreeActions.ToggleNodeLoading, (state, action) => ({
		...state,
		nodes: toggleNodeLoadingToExpand(state.nodes, action.payload.node.dcvId)
	})),
	on(TreeActions.SelectNode, (state, action) => ({
		...state,
		nodes: selectNode(state.nodes, action.payload.topicId)
	})),
	on(TreeActions.RemoveCreateState, (state) => ({
		...state,
		nodes: [...state.nodes.map((t) => ({ ...t, isCreated: false }))]
	})),
	on(TreeEditActions.MoveToTopSuccess, (state, action) => ({
		...state,
		nodes: moveNodeToTop(state.nodes, action.payload.topicId)
	})),
	on(TreeEditActions.MoveToBottomSuccess, (state, action) => ({
		...state,
		nodes: moveNodeToBottom(state.nodes, action.payload.topicId)
	})),
	on(TreeEditActions.MoveUpSuccess, (state, action) => ({
		...state,
		nodes: moveNodeUp(state.nodes, action.payload.topicId)
	})),
	on(TreeEditActions.MoveDownSuccess, (state, action) => ({
		...state,
		nodes: moveNodeDown(state.nodes, action.payload.topicId)
	})),
	on(TreeEditActions.MoveLevelDownSuccess, (state, action) => ({
		...state,
		nodes: moveNodeLevelDown(state.nodes, action.payload.topicId)
	})),
	on(TreeEditActions.MoveLevelUpSuccess, (state, action) => ({
		...state,
		nodes: moveNodeLevelUp(state.nodes, action.payload.topicId)
	}))
);

const addNodeUnder = (
	nodes: FlatTreeNode[],
	topic: Topic,
	topicIdAbove: string
): FlatTreeNode[] => {
	const newNodes: FlatTreeNode[] = nodes.map((node) => ({ ...node }));
	const nodeIndex = newNodes.findIndex((node) => node.dcvId === topicIdAbove);
	if (nodeIndex < 0) {
		return newNodes;
	}

	const newNode: FlatTreeNode = {
		...mapTopic(topic, nodes[nodeIndex].level),
		isCreated: true
	};
	const nextIndex =
		nodeIndex +
		getChildCount(newNodes, nodeIndex, newNodes[nodeIndex].level) +
		1;
	newNodes.splice(nextIndex, 0, newNode);
	return newNodes;
};

const createChildNode = (
	nodes: FlatTreeNode[],
	topic: Topic,
	parrentTopic: string
): FlatTreeNode[] => {
	const newNodes: FlatTreeNode[] = nodes.map((node) => ({ ...node }));
	const parentIndex = newNodes.findIndex(
		(node) => node.dcvId === parrentTopic
	);

	if (parentIndex < 0) {
		return newNodes;
	}

	const childLevel = newNodes[parentIndex].level + 1;

	newNodes[parentIndex].isExpandable = true;
	newNodes[parentIndex].isExpanded = true;
	newNodes[parentIndex].isLoading = false;

	const childNode: FlatTreeNode = {
		...mapTopic(topic, childLevel),
		isCreated: true
	};
	const nextIndex =
		parentIndex +
		getChildCount(newNodes, parentIndex, newNodes[parentIndex].level) +
		1;
	newNodes.splice(nextIndex, 0, childNode);
	return newNodes;
};

const addChildNodes = (
	nodes: FlatTreeNode[],
	children: FlatTreeNode[],
	dcvId: string
): FlatTreeNode[] => {
	const newNodes: FlatTreeNode[] = nodes.map((node) => ({ ...node }));
	const parentIndex = newNodes.findIndex((node) => node.dcvId === dcvId);
	newNodes[parentIndex] = { ...newNodes[parentIndex], isExpanded: true };
	newNodes.splice(parentIndex + 1, 0, ...[...children].sort(orderByNumber));
	return newNodes;
};

const deleteNode = (
	nodes: FlatTreeNode[],
	topicId: string,
	recycleBinId: string
): FlatTreeNode[] => {
	const newNodes: FlatTreeNode[] = nodes.map((node) => ({ ...node }));
	const topicIndex = newNodes.findIndex((node) => node.dcvId === topicId);
	const recycleBinIndex = newNodes.findIndex(
		(node) => node.dcvId === recycleBinId
	);
	if (topicIndex < 0) {
		return newNodes;
	}
	const parentIndex = getParentIndexFromChildIndex(newNodes, topicId);
	const parentDirectChildCount = getDirectChildCount(
		newNodes,
		parentIndex,
		newNodes[parentIndex].level
	);

	newNodes[topicIndex].isLoading = false;
	const isTopicToMoveInRecycleBin = newNodes[topicIndex].isInRecycleBin;
	const nodeChildCount = getChildCount(
		newNodes,
		topicIndex,
		newNodes[topicIndex].level
	);
	const nodesToMove = newNodes.splice(topicIndex, nodeChildCount + 1);

	if (parentDirectChildCount === 1) {
		newNodes[parentIndex].isExpanded = false;
		newNodes[parentIndex].isExpandable = false;
	}
	if (newNodes[recycleBinIndex]?.isExpanded && !isTopicToMoveInRecycleBin) {
		const levelOffset =
			newNodes[recycleBinIndex].level - nodesToMove[0].level + 1;
		const updatedNodesToMove = nodesToMove.map((node) => {
			return {
				...node,
				level: node.level + levelOffset,
				isInRecycleBin: true,
				canCreateChildTopic: false,
				canCreateTopicAfter: false
			};
		});
		newNodes.splice(recycleBinIndex + 1, 0, ...[...updatedNodesToMove]);
	}

	if (!newNodes[recycleBinIndex]?.isExpandable && !isTopicToMoveInRecycleBin)
		newNodes[recycleBinIndex].isExpandable = true;

	return newNodes;
};

const removeChildNodes = (
	nodes: FlatTreeNode[],
	level: number,
	dcvId: string
): FlatTreeNode[] => {
	const newNodes: FlatTreeNode[] = nodes.map((node) => ({ ...node }));
	const parentIndex = newNodes.findIndex((node) => node.dcvId === dcvId);
	newNodes[parentIndex] = { ...newNodes[parentIndex], isExpanded: false };
	let count = 0;

	// count the amount of childs in the node
	for (let i = parentIndex + 1; i < newNodes.length; i++) {
		// break when a non-child node is encountered
		if (level >= newNodes[i].level) {
			break;
		}
		count++;
	}
	// remove the counted childs from the list, starting from the first child
	newNodes.splice(parentIndex + 1, count);

	return newNodes;
};

const toggleNodeLoadingToExpand = (
	nodes: FlatTreeNode[],
	dcvId: string
): FlatTreeNode[] =>
	nodes.map((node) => {
		if (node.dcvId === dcvId) {
			const isLoading = !node.isLoading;
			return {
				...node,
				isExpandable: true,
				isExpanded: !isLoading,
				isLoading
			};
		}
		return node;
	});

const toggleNodeLoading = (
	nodes: FlatTreeNode[],
	dcvId: string
): FlatTreeNode[] =>
	nodes.map((node) => {
		if (node.dcvId === dcvId) {
			return { ...node, isLoading: !node.isLoading };
		}
		return node;
	});

const selectNode = (nodes: FlatTreeNode[], dcvId: string): FlatTreeNode[] =>
	nodes.map((node) => ({ ...node, isSelected: node.dcvId === dcvId }));

const updateNodeName = (
	nodes: FlatTreeNode[],
	dcvId: string,
	name: string
): FlatTreeNode[] =>
	nodes.map((node) => {
		if (node.dcvId === dcvId) {
			return { ...node, item: name };
		}
		return node;
	});

const mapPath = (path: Path): FlatTreeNode[] => {
	const flatTreeNodes: FlatTreeNode[] = [];

	const PathItems = path.path.slice().sort((a, b) => a.order - b.order);
	const DictionaryTopics: Dictionary<Topic> = topicArrayToDictionary(
		path.data
	);

	PathItems.forEach((item, level, pathItems) => {
		const isRootTopic = level === 0;
		const lastItemFromPath = pathItems[pathItems.length - 1].dcvId;
		const {
			name,
			dcv,
			icon,
			orderNumber,
			customIconId,
			hasChildren,
			business,
			isInRecycleBin
		} = DictionaryTopics[item.dcvId];

		if (isRootTopic) {
			flatTreeNodes.push(
				new FlatTreeNode(
					name,
					dcv,
					icon,
					level,
					orderNumber,
					customIconId,
					hasChildren,
					business?.canDelete,
					isInRecycleBin,
					business?.canCreateChildTopic,
					business?.canCreateTopicAfter,
					dcv === lastItemFromPath
				)
			);
		}

		if (hasChildren) {
			const nextIndex = level + 1;
			const children = getChilds(path.data, dcv, level)
				.sort(orderByNumber)
				.map((child) => {
					if (nextIndex < pathItems.length) {
						const nextItemDcvId = pathItems[level + 1].dcvId;
						if (child.dcvId === nextItemDcvId) {
							return {
								...child,
								isExpanded: true,
								isSelected: nextItemDcvId === lastItemFromPath
							};
						}
					}
					return child;
				});

			const previousSelectedItemIndex = flatTreeNodes.findIndex(
				(n) => n.dcvId === item.dcvId
			);
			flatTreeNodes.splice(previousSelectedItemIndex + 1, 0, ...children);
		}
	});
	return flatTreeNodes;
};

const getChilds = (
	data: Topic[],
	topicId: string,
	level: number
): FlatTreeNode[] => {
	return data
		.filter((node) => node.parent === topicId)
		.map((node) => {
			const {
				name,
				dcv,
				icon,
				orderNumber,
				customIconId,
				hasChildren,
				isInRecycleBin,
				business
			} = node;
			return new FlatTreeNode(
				name,
				dcv,
				icon,
				level + 1,
				orderNumber,
				customIconId,
				hasChildren,
				business?.canDelete,
				isInRecycleBin,
				business?.canCreateChildTopic,
				business?.canCreateTopicAfter
			);
		});
};

const topicArrayToDictionary = (topics: Topic[]): Dictionary<Topic> =>
	topics.reduce((dic, obj) => {
		dic[obj.dcv] = obj;
		return dic;
	}, {});

const orderByNumber = (a: FlatTreeNode, b: FlatTreeNode): number => {
	if (a.orderNumber < b.orderNumber) {
		return -1;
	}
	if (a.orderNumber > b.orderNumber) {
		return 1;
	}
	return 0;
};

const moveNodeToTop = (
	nodes: FlatTreeNode[],
	topicId: string
): FlatTreeNode[] => {
	const newNodes: FlatTreeNode[] = nodes.map((node) => ({ ...node }));
	const parentIndex = getParentIndexFromChildIndex(newNodes, topicId);
	const nodeIndex = nodes.findIndex((node) => node.dcvId === topicId);
	if (parentIndex === undefined || nodeIndex < 0) {
		return newNodes;
	}

	const nodeChildCount = getChildCount(
		newNodes,
		nodeIndex,
		newNodes[nodeIndex].level
	);

	const nodesToMove = newNodes.splice(nodeIndex, nodeChildCount + 1);
	newNodes.splice(parentIndex + 1, 0, ...[...nodesToMove]);

	return newNodes;
};

const moveNodeToBottom = (
	nodes: FlatTreeNode[],
	topicId: string
): FlatTreeNode[] => {
	const newNodes: FlatTreeNode[] = nodes.map((node) => ({ ...node }));
	const lastSiblingIndex = getLastSiblingIndexFromChildIndex(
		newNodes,
		topicId
	);
	const nodeIndex = nodes.findIndex((node) => node.dcvId === topicId);
	if (lastSiblingIndex === undefined || nodeIndex < 0) {
		return newNodes;
	}

	const nodeChildCount = getChildCount(
		newNodes,
		nodeIndex,
		newNodes[nodeIndex].level
	);

	const nodesToMove = newNodes.splice(nodeIndex, nodeChildCount + 1);
	newNodes.splice(lastSiblingIndex - nodeChildCount, 0, ...[...nodesToMove]);

	return newNodes;
};

const moveNodeUp = (nodes: FlatTreeNode[], topicId: string): FlatTreeNode[] => {
	const newNodes: FlatTreeNode[] = nodes.map((node) => ({ ...node }));
	const siblingAboveIndex = getSiblingAboveIndexFromChildIndex(
		newNodes,
		topicId
	);
	const nodeIndex = nodes.findIndex((node) => node.dcvId === topicId);
	if (siblingAboveIndex === undefined || nodeIndex < 0) {
		return newNodes;
	}

	const nodeChildCount = getChildCount(
		newNodes,
		nodeIndex,
		newNodes[nodeIndex].level
	);

	const nodesToMove = newNodes.splice(nodeIndex, nodeChildCount + 1);
	newNodes.splice(siblingAboveIndex, 0, ...[...nodesToMove]);

	return newNodes;
};

const moveNodeDown = (
	nodes: FlatTreeNode[],
	topicId: string
): FlatTreeNode[] => {
	const newNodes: FlatTreeNode[] = nodes.map((node) => ({ ...node }));
	const siblingBelowIndex = getSiblingBelowIndexFromChildIndex(
		newNodes,
		topicId
	);
	if (siblingBelowIndex === undefined) {
		return newNodes;
	}

	return moveNodeUp(nodes, nodes[siblingBelowIndex].dcvId);
};

const moveNodeLevelDown = (
	nodes: FlatTreeNode[],
	topicId: string
): FlatTreeNode[] => {
	const newNodes: FlatTreeNode[] = nodes.map((node) => ({ ...node }));
	const nodeIndex = nodes.findIndex((node) => node.dcvId === topicId);
	if (nodeIndex < 0) {
		return newNodes;
	}

	const nodeChildCount = getChildCount(
		newNodes,
		nodeIndex,
		newNodes[nodeIndex].level
	);

	for (let i = nodeIndex; i <= nodeIndex + nodeChildCount; i++) {
		newNodes[i].level += 1;
	}

	const siblingAboveIndex = getSiblingAboveIndexFromChildIndex(
		nodes,
		topicId
	);
	newNodes[siblingAboveIndex].isExpandable = true;
	newNodes[siblingAboveIndex].isExpanded = true;

	return newNodes;
};

const moveNodeLevelUp = (
	nodes: FlatTreeNode[],
	topicId: string
): FlatTreeNode[] => {
	const newNodes: FlatTreeNode[] = nodes.map((node) => ({ ...node }));
	const parentIndex = getParentIndexFromChildIndex(nodes, topicId);
	const nodeIndex = nodes.findIndex((node) => node.dcvId === topicId);
	if (parentIndex === undefined || nodeIndex < 0) {
		return newNodes;
	}

	const nodeChildCount = getChildCount(
		newNodes,
		nodeIndex,
		newNodes[nodeIndex].level
	);

	for (let i = nodeIndex; i <= nodeIndex + nodeChildCount; i++) {
		newNodes[i].level -= 1;
	}

	const nodesToMove = newNodes.splice(nodeIndex, nodeChildCount + 1);
	const parentChildCount = getChildCount(
		newNodes,
		parentIndex,
		newNodes[parentIndex].level
	);

	newNodes.splice(parentIndex + parentChildCount + 1, 0, ...[...nodesToMove]);
	newNodes[parentIndex].isExpandable = parentChildCount > 0;

	return newNodes;
};

const getChildCount = (
	nodes: FlatTreeNode[],
	parentIndex: number,
	level: number
): number => {
	let count = 0;

	// count the amount of childs in the node
	for (let i = parentIndex + 1; i < nodes.length; i++) {
		// break when a non-child node is encountered
		if (level >= nodes[i].level) {
			break;
		}
		count++;
	}

	return count;
};

const getDirectChildCount = (
	nodes: FlatTreeNode[],
	parentIndex: number,
	level: number
): number => {
	let count = 0;
	// count the amount of childs in the node
	for (let i = parentIndex + 1; i < nodes.length; i++) {
		// break when a non-child node is encountered
		if (level >= nodes[i].level) {
			break;
		}
		if (nodes[i].level - level === 1) count++;
	}

	return count;
};

const getParentIndexFromChildIndex = (
	nodes: FlatTreeNode[],
	topicId: string
): number => {
	const nodeIndex = nodes.findIndex((node) => node.dcvId === topicId);
	if (nodeIndex > 0) {
		for (let i = nodeIndex - 1; i >= 0; i--) {
			if (nodes[i].level < nodes[nodeIndex].level) {
				return i;
			}
		}
	}

	return undefined;
};

const getLastSiblingIndexFromChildIndex = (
	nodes: FlatTreeNode[],
	topicId: string
): number => {
	const nodeIndex = nodes.findIndex((node) => node.dcvId === topicId);
	if (nodeIndex > 0) {
		for (let i = nodeIndex + 1; i < nodes.length; i++) {
			if (nodes[i].level < nodes[nodeIndex].level) {
				return i - 1;
			}
		}
		return nodes.length - 1;
	}

	return undefined;
};

const getSiblingAboveIndexFromChildIndex = (
	nodes: FlatTreeNode[],
	topicId: string
): number => {
	const nodeIndex = nodes.findIndex((node) => node.dcvId === topicId);
	if (nodeIndex > 0) {
		for (let i = nodeIndex - 1; i >= 0; i--) {
			if (nodes[i].level === nodes[nodeIndex].level) {
				return i;
			}
		}
	}

	return undefined;
};

const getSiblingBelowIndexFromChildIndex = (
	nodes: FlatTreeNode[],
	topicId: string
): number => {
	const nodeIndex = nodes.findIndex((node) => node.dcvId === topicId);
	if (nodeIndex > 0) {
		for (let i = nodeIndex + 1; i < nodes.length; i++) {
			if (nodes[i].level === nodes[nodeIndex].level) {
				return i;
			}
		}
	}

	return undefined;
};

const mapTopic = (topic: Topic, level: number) => {
	const {
		name,
		dcv,
		icon,
		orderNumber,
		customIconId,
		hasChildren,
		isInRecycleBin,
		business
	} = topic;
	return new FlatTreeNode(
		name,
		dcv,
		icon,
		level,
		orderNumber,
		customIconId,
		hasChildren,
		business?.canDelete,
		isInRecycleBin,
		business?.canCreateChildTopic,
		business?.canCreateTopicAfter
	);
};
