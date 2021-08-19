import { FetchStatus } from '../../enums/FetchState';
import { FlatTreeNode } from '../models/flat-tree-node.model';

export interface TreeState {
	fetchStatus: FetchStatus;
	selectedNode: string;
	nodes: FlatTreeNode[];
}
