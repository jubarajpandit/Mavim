import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import * as actions from '../+state/actions/topic.actions';
import { Store } from '@ngrx/store';
import * as topic from '../+state/selectors/topic.selector';
import { Topic } from '../models/topic.model';

@Injectable({ providedIn: 'root' })
export class TopicFacade {
	public constructor(private readonly store: Store) {}

	public get getTopicRoot(): Observable<Topic> {
		return this.store.select(topic.selectRootElement);
	}

	public get getRecycleBin(): Observable<Topic> {
		return this.store.select(topic.selectRecycleBin);
	}

	public get getCategories(): Observable<Topic[]> {
		return this.store.select(topic.selectCategories);
	}

	public get getAllTopicsLoaded(): Observable<boolean> {
		return this.store.select(topic.allTopicsLoaded);
	}

	public getTopicByDcv(dcvid: string): Observable<Topic> {
		return this.store.select(topic.selectTopicById(dcvid));
	}

	public getChildTopicsByDcv(dcvid: string): Observable<Topic[]> {
		return this.store.select(topic.selectChildTopicsByDcv(dcvid));
	}

	public loadTopicByDcv(dcvid: string): void {
		this.store.dispatch(actions.LoadTopicByDCV({ payload: dcvid }));
	}

	public loadRoot(): void {
		this.store.dispatch(actions.LoadTopicRoot());
	}
	public loadCategories(): void {
		this.store.dispatch(actions.LoadRelationshipTypes());
	}

	public loadTopicChildren(dcvId: string): void {
		this.store.dispatch(actions.LoadTopicChildren({ payload: dcvId }));
	}

	public createTopic(
		topicId: string,
		name: string,
		type: string,
		icon: string
	): void {
		this.store.dispatch(
			actions.CreateTopic({ payload: { topicId, name, type, icon } })
		);
	}

	public createChildtopic(
		parentTopicId: string,
		name: string,
		type: string,
		icon: string
	): void {
		this.store.dispatch(
			actions.CreateChildTopic({
				payload: { parentTopicId, name, type, icon }
			})
		);
	}

	public deleteTopic(topicId: string, recycleBinId: string): void {
		this.store.dispatch(
			actions.DeleteTopic({ payload: { topicId, recycleBinId } })
		);
	}
}
