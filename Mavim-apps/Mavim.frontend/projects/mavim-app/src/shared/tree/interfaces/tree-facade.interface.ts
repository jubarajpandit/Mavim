import { FlatTreeNode } from '../models/flat-tree-node.model';
import { Observable } from 'rxjs';

export interface TreeFacadeInterface {
	selectNodes: Observable<FlatTreeNode[]>;
}
