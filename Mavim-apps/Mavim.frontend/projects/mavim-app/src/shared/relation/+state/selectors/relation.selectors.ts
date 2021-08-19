import * as fromRelations from '../reducers/relation.reducers';
import { RelationState } from '../../interfaces/relation-state.interface';
import {
	createFeatureSelector,
	createSelector,
	DefaultProjectorFn,
	MemoizedSelector
} from '@ngrx/store';
import { Relation } from '../../models/relation.model';

export const selectRelationState =
	createFeatureSelector<RelationState>('relations');

export const selectRelationsByDcv = (
	dcv: string
): MemoizedSelector<
	RelationState,
	Relation[],
	DefaultProjectorFn<Relation[]>
> =>
	createSelector(selectRelationState, (relationState) =>
		relationState.allRelationsLoaded
			? dcvStartsWith(relationState, dcv)
			: undefined
	);

export const selectAllRelations = createSelector(
	selectRelationState,
	fromRelations.selectAllRelations
);

export const allRelationsLoaded = createSelector(
	selectRelationState,
	(relationState) => relationState.allRelationsLoaded
);

function dcvStartsWith(state: RelationState, dcv): Relation[] {
	const relations: Relation[] = [];
	(state.ids as string[]).map((id) => {
		if (id.startsWith(dcv)) {
			relations.push(state.entities[id]);
		}
	});
	return relations;
}
