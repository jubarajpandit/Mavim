import { createAction, props } from '@ngrx/store';
import { FlatTreeNode } from '../models/flat-tree-node.model';
import { Path } from '../models/api/path';

export const ClearTree = createAction(
	'[Generic Tree] Clear tree state',
	props<{ payload: { namespace: string } }>()
);
export const ExpandToNode = createAction(
	'[Generic Tree] Expand to node',
	props<{ payload: { namespace: string; topicId: string } }>()
);
export const ExpandToNodeSuccess = createAction(
	'[Generic Tree] Expand to node Success',
	props<{
		payload: {
			namespace: string;
			path: Path;
		};
	}>()
);
export const ExpandToNodeFailed = createAction(
	'[Generic Tree] Expand to node Failed',
	props<{ payload: { namespace: string } }>()
);
export const ExpandNode = createAction(
	'[Generic Tree] Expand Node',
	props<{ payload: { namespace: string; topicId: string } }>()
);
export const AddChildNodes = createAction(
	'[Generic Tree] Add children to node',
	props<{
		payload: {
			namespace: string;
			parent: FlatTreeNode;
			children: FlatTreeNode[];
		};
	}>()
);
export const RemoveChildNodes = createAction(
	'[Generic Tree] Remove children of node',
	props<{ payload: { namespace: string; node: FlatTreeNode } }>()
);
export const ToggleNodeLoading = createAction(
	'[Generic Tree] Toggle node loading',
	props<{ payload: { namespace: string; node: FlatTreeNode } }>()
);
export const SelectNode = createAction(
	'[Generic Tree] Select node',
	props<{ payload: { namespace: string; topicId: string } }>()
);
export const RemoveCreateState = createAction(
	'[Generic Tree] Remove Create State',
	props<{ payload: { namespace: string; topicId: string } }>()
);
