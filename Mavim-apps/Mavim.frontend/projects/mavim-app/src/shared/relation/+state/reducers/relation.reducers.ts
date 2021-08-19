import { RootState } from '../../../../app/+state';
import { createEntityAdapter, EntityAdapter } from '@ngrx/entity';
import * as relationActions from '../actions/relation.actions';
import { RelationState } from '../../interfaces/relation-state.interface';
import { Relation } from '../../models/relation.model';
import { createReducer, on } from '@ngrx/store';
import * as languageActions from '../../../language/+state';

export interface FeatureState extends RootState {
	relations: RelationState;
}

export const relationsAdapter: EntityAdapter<Relation> =
	createEntityAdapter<Relation>({
		// refactor relation dcv: WI 16192
		selectId: (relation) => `${relation.topicDCV}_${relation.dcv}`
	});

const initialRelationState: RelationState = relationsAdapter.getInitialState({
	allRelationsLoaded: false
});

export const reducer = createReducer(
	initialRelationState,
	on(
		relationActions.LoadRelations,
		relationActions.CreateRelation,
		relationActions.DeleteRelation,
		(state): RelationState => ({
			...state,
			allRelationsLoaded: false
		})
	),
	on(relationActions.LoadRelationsSuccess, (state, { payload }) =>
		relationsAdapter.addMany(payload, {
			...state,
			allRelationsLoaded: true
		})
	),
	on(relationActions.CreateRelationSuccess, (state, { payload }) =>
		relationsAdapter.addOne(payload, { ...state, allRelationsLoaded: true })
	),
	on(relationActions.DeleteRelationSuccess, (state, { payload }) =>
		relationsAdapter.removeOne(payload, {
			...state,
			allRelationsLoaded: true
		})
	),
	on(
		relationActions.LoadRelationsFail,
		relationActions.CreateRelationFail,
		relationActions.DeleteRelationFail,
		(state): RelationState => ({ ...state, allRelationsLoaded: true })
	),
	on(
		languageActions.UpdateLanguage,
		(): RelationState => ({ ...initialRelationState })
	)
);

export const {
	selectAll: selectAllRelations,
	selectEntities: selectRelationEntities,
	selectIds: selectRelationIds,
	selectTotal: relationsCount
} = relationsAdapter.getSelectors();
