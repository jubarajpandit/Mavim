import { Dictionary } from '@ngrx/entity';
import { TopicMetaType } from '../models/topic-meta-type.model';

export interface TopicMetaState {
	TypesLoaded: boolean;
	IconsLoaded: boolean;
	Types: Dictionary<TopicMetaType>;
	Icons: Dictionary<string>;
}
