import { Store } from '@ngrx/store';
import { FlatTreeNode } from '../models/flat-tree-node.model';
import { Observable } from 'rxjs';
import { TreeFacadeInterface } from '../interfaces/tree-facade.interface';
import * as actions from '../+state/generictree.actions';
import * as editActions from '../+state/tree-edit.actions';
import { selectNodes, selectFetchStatus } from '../+state/generictree.selector';
import { FetchStatus } from '../../enums/FetchState';

export abstract class GenericTreeFacade implements TreeFacadeInterface {
	public constructor(
		protected readonly store: Store,
		protected readonly namespace: string
	) {}

	public get selectNodes(): Observable<FlatTreeNode[]> {
		return this.store.select(selectNodes(this.namespace));
	}

	public get selectFetchStatus(): Observable<FetchStatus> {
		return this.store.select(selectFetchStatus(this.namespace));
	}

	public ClearTree(): void {
		this.store.dispatch(
			actions.ClearTree({ payload: { namespace: this.namespace } })
		);
	}

	public ExpandTo(topicId: string): void {
		this.store.dispatch(
			actions.ExpandToNode({
				payload: {
					namespace: this.namespace,
					topicId
				}
			})
		);
	}

	public ExpandNode(topicId: string): void {
		this.store.dispatch(
			actions.ExpandNode({
				payload: {
					namespace: this.namespace,
					topicId
				}
			})
		);
	}

	public Add(parent: FlatTreeNode, children: FlatTreeNode[]): void {
		this.store.dispatch(
			actions.AddChildNodes({
				payload: {
					namespace: this.namespace,
					parent,
					children
				}
			})
		);
	}

	public Remove(node: FlatTreeNode): void {
		this.store.dispatch(
			actions.RemoveChildNodes({
				payload: {
					namespace: this.namespace,
					node
				}
			})
		);
	}

	public ToggleLoading(node: FlatTreeNode): void {
		this.store.dispatch(
			actions.ToggleNodeLoading({
				payload: {
					namespace: this.namespace,
					node
				}
			})
		);
	}

	public select(topicId: string): void {
		this.store.dispatch(
			actions.SelectNode({
				payload: { namespace: this.namespace, topicId }
			})
		);
	}

	public moveToTop(topicId: string): void {
		this.store.dispatch(
			editActions.MoveToTop({
				payload: {
					namespace: this.namespace,
					topicId
				}
			})
		);
	}

	public moveToBottom(topicId: string): void {
		this.store.dispatch(
			editActions.MoveToBottom({
				payload: {
					namespace: this.namespace,
					topicId
				}
			})
		);
	}

	public moveUp(topicId: string): void {
		this.store.dispatch(
			editActions.MoveUp({
				payload: { namespace: this.namespace, topicId }
			})
		);
	}

	public moveDown(topicId: string): void {
		this.store.dispatch(
			editActions.MoveDown({
				payload: {
					namespace: this.namespace,
					topicId
				}
			})
		);
	}

	public moveLevelUp(topicId: string): void {
		this.store.dispatch(
			editActions.MoveLevelUp({
				payload: {
					namespace: this.namespace,
					topicId
				}
			})
		);
	}

	public moveLevelDown(topicId: string): void {
		this.store.dispatch(
			editActions.MoveLevelDown({
				payload: {
					namespace: this.namespace,
					topicId
				}
			})
		);
	}
}
