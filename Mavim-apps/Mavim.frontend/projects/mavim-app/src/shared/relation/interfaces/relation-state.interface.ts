import { Relation } from '../models/relation.model';
import { EntityState } from '@ngrx/entity';

export interface RelationState extends EntityState<Relation> {
	allRelationsLoaded: boolean;
}
