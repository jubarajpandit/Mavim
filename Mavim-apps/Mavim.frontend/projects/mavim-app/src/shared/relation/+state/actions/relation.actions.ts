import { createAction, props } from '@ngrx/store';
import { Relation } from '../../models/relation.model';

export const LoadRelations = createAction(
	'[Relation] Load relations',
	props<{ payload: string }>()
);
export const LoadRelationsSuccess = createAction(
	'[Relation] Load relations success',
	props<{ payload: Relation[] }>()
);
export const LoadRelationsFail = createAction('[Relation] Load relations fail');

export const CreateRelation = createAction(
	'[Relation] Create Relation',
	props<{ payload: Relation }>()
);
export const CreateRelationSuccess = createAction(
	'[Relation] Create Relation Success',
	props<{ payload: Relation }>()
);
export const CreateRelationFail = createAction(
	'[Relation] Create Relation Fail'
);

export const DeleteRelation = createAction(
	'[Relation] Delete relation',
	props<{ payload: Relation }>()
);
export const DeleteRelationSuccess = createAction(
	'[Relation] Delete Relation Success',
	props<{ payload: string }>()
);
export const DeleteRelationFail = createAction(
	'[Relation] Delete relation fail'
);
