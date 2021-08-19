import { createAction, props } from '@ngrx/store';
import { Field } from '../../models/field.model';

export const LoadFields = createAction(
	'[Field] Load fields',
	props<{ payload: string }>()
);
export const LoadFieldsFail = createAction('[Field] Load fields fail');
export const LoadFieldsSuccess = createAction(
	'[Field] Load fields success',
	props<{ payload: Field[] }>()
);

export const UpdateField = createAction(
	'[Field] Update Field',
	props<{ payload: Field }>()
);
export const UpdateFieldFail = createAction(
	'[Field] Update Fieds failed',
	props<{ payload: Field }>()
);
export const UpdateFieldSuccess = createAction('[Field] Update Field Success');

export const UpdateFields = createAction(
	'[Fields] Update Fields',
	props<{ payload: Field[] }>()
);
export const UpdateFieldsFail = createAction(
	'[Fields] Update Fields failed',
	props<{ payload: Field[] }>()
);
export const UpdateFieldsSuccess = createAction(
	'[Fields] Update Field Success'
);
