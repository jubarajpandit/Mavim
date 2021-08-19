import { Injectable } from '@angular/core';
import { Dictionary } from '@ngrx/entity';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import * as actions from '../+state/actions/topic-meta.actions';
import {
	selectIcons,
	selectIconsLoaded,
	selectTypes,
	selectTypesLoaded
} from '../+state/selectors/topic-meta.selectors';
import { TopicMetaType } from '../models/topic-meta-type.model';

@Injectable({ providedIn: 'root' })
export class TopicMetaFacade {
	public constructor(private readonly store: Store) {}

	public get getTypesLoaded(): Observable<boolean> {
		return this.store.select(selectTypesLoaded);
	}

	public get getTypes(): Observable<Dictionary<TopicMetaType>> {
		return this.store.select(selectTypes);
	}

	public get getIconsLoaded(): Observable<boolean> {
		return this.store.select(selectIconsLoaded);
	}

	public get getIcons(): Observable<Dictionary<string>> {
		return this.store.select(selectIcons);
	}

	public loadTypes(topicId: string): void {
		this.store.dispatch(actions.LoadTopicTypes({ payload: topicId }));
	}

	public loadIcons(elementType: string): void {
		this.store.dispatch(actions.LoadTopicIcons({ payload: elementType }));
	}

	public clearStore(): void {
		this.store.dispatch(actions.ClearTopicMetaStore());
	}
}
