import {
	createFeatureSelector,
	createSelector,
	DefaultProjectorFn,
	MemoizedSelector
} from '@ngrx/store';
import { FetchStatus } from '../../enums/FetchState';
import { TreeState } from '../interfaces/tree-state.interface';
import { FlatTreeNode } from '../models/flat-tree-node.model';

const selectTreeState = (namespace: string) =>
	createFeatureSelector<TreeState>(namespace);

export const selectNodes = (
	namespace: string
): MemoizedSelector<
	unknown,
	FlatTreeNode[],
	DefaultProjectorFn<FlatTreeNode[]>
> =>
	createSelector(
		selectTreeState(namespace),
		(treeState: TreeState) => treeState.nodes
	);

export const selectFetchStatus = (
	namespace: string
): MemoizedSelector<TreeState, FetchStatus, DefaultProjectorFn<FetchStatus>> =>
	createSelector(
		selectTreeState(namespace),
		(treeState: TreeState) => treeState.fetchStatus
	);
