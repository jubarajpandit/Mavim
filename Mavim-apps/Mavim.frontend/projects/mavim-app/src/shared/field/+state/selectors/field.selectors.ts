import * as fromFields from '../reducers/field.reducers';
import { FieldState } from '../../interfaces/field-state.interface';
import {
	createFeatureSelector,
	createSelector,
	DefaultProjectorFn,
	MemoizedSelector
} from '@ngrx/store';
import { Field } from '../../models/field.model';
import { Dictionary } from '@ngrx/entity';

export const selectFieldState = createFeatureSelector<FieldState>('fields');

export const selectFieldByDcv = (
	dcv: string
): MemoizedSelector<FieldState, Field[], DefaultProjectorFn<Field[]>> =>
	createSelector(selectFieldState, (fieldsState) =>
		fieldsState.allFieldsLoaded
			? idStartsWith(fieldsState, dcv).sort(orderBySetOrderAndFieldOrder)
			: undefined
	);

export const selectAllFields = createSelector(
	selectFieldState,
	fromFields.selectAllFields
);

export const allFieldsLoaded = createSelector(
	selectFieldState,
	(fieldsState) => fieldsState.allFieldsLoaded
);

export const selectFieldByID = (
	entitieId: string
): MemoizedSelector<FieldState, Field, DefaultProjectorFn<Field>> =>
	createSelector(
		selectFieldState,
		(fieldsState) => fieldsState.entities[entitieId]
	);

export const selectFieldsByID = (
	entitieIds: string[]
): MemoizedSelector<FieldState, Field[], DefaultProjectorFn<Field[]>> =>
	createSelector(selectFieldState, (fieldsState) => {
		const entities = dictionaryFilter(fieldsState.entities, entitieIds);
		return Object.keys(entities).map((key) => {
			return entities[key];
		});
	});

const dictionaryFilter = (
	fields: Dictionary<Field>,
	ids: string[]
): Dictionary<Field> => {
	return Object.keys(fields)
		.filter((key) => ids.includes(key))
		.reduce((obj, key) => {
			return {
				...obj,
				[key]: fields[key]
			};
		}, {});
};

function idStartsWith(state: FieldState, dcv: string): Field[] {
	const fields: Field[] = [];
	(state.ids as string[]).map((id) => {
		if (id.startsWith(dcv)) {
			fields.push(state.entities[id]);
		}
	});
	return fields;
}

const orderBySetOrderAndFieldOrder = (a: Field, b: Field): number => {
	if (a.setOrder === b.setOrder) {
		return a.order - b.order;
	}
	return a.setOrder > b.setOrder ? 1 : -1;
};
