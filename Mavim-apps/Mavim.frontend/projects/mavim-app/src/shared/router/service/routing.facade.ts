import { Injectable } from '@angular/core';
import { Outlet } from '../models/outlet';
import { Store } from '@ngrx/store';
import * as actions from '../+state/routing.actions';
import { selectCurrentQueue } from '../+state/router.selectors';
import { Observable } from 'rxjs';
import { Topic } from '../../topic/models/topic.model';

@Injectable()
export class RoutingFacade {
	public constructor(private readonly store: Store) {}

	/**
	 * Initializes the store by setting the current dcv ids
	 */
	public init(currentTopicIds: string[]): void {
		this.store.dispatch(actions.Init({ payload: currentTopicIds }));
	}

	/**
	 * Gets the queue (list of dcvs) from the store
	 */
	public get queue(): Observable<string[]> {
		return this.store.select(selectCurrentQueue);
	}

	/**
	 * Go to Home screen
	 */
	public home(topicId: string): void {
		this.store.dispatch(actions.Home({ payload: topicId }));
	}

	/**
	 * Display next screen in right outlet
	 * Copy (old) right outlet and shift to left
	 */
	public next(dcvId: string, outlet: Outlet): void {
		this.store.dispatch(actions.Next({ payload: { dcvId, outlet } }));
	}

	/**
	 * Display previous screen in the left and right outlet
	 * Shifts the screens to the right
	 */
	public back(): void {
		this.store.dispatch(actions.Back());
	}

	/**
	 * Go to edit screen based on dcvID
	 * Copy previous outlet for history
	 */
	public edit(topicId: string): void {
		this.store.dispatch(actions.Edit({ payload: topicId }));
	}

	/**
	 * Go to the word edit screen based on dcvId
	 */
	public editWord(topicId: string): void {
		this.store.dispatch(actions.EditWord({ payload: topicId }));
	}

	/**
	 * Go to the users edit screen
	 */
	public editUsers(): void {
		this.store.dispatch(actions.EditUsers());
	}

	/**
	 * Go to the word edit screen based on dcvId
	 */
	public testWopi(topicId: string): void {
		this.store.dispatch(actions.TestWopi({ payload: topicId }));
	}

	/**
	 * Go to the create new Word screen based on dcvId
	 */
	public createNewWord(topic: Topic): void {
		this.store.dispatch(actions.CreateNewWord({ payload: topic }));
	}

	/**
	 * Go back to splitscreen from Edit page
	 */
	public backEdit(): void {
		this.store.dispatch(actions.BackEdit());
	}

	/**
	 * Update Queue after browser navigates
	 */
	public updateQueue(topicIds: string[]): void {
		this.store.dispatch(actions.UpdateQueue({ payload: topicIds }));
	}
}
