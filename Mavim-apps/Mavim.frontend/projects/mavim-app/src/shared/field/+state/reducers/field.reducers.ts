import { RootState } from '../../../../app/+state';
import { createEntityAdapter, EntityAdapter, Update } from '@ngrx/entity';
import * as fieldActions from '../actions/field.actions';
import { FieldState } from '../../interfaces/field-state.interface';
import { Field } from '../../models/field.model';
import { createReducer, on } from '@ngrx/store';
import * as languageActions from '../../../language/+state';

export interface FeatureState extends RootState {
	fields: FieldState;
}

export const fieldsAdapter: EntityAdapter<Field> = createEntityAdapter<Field>({
	selectId: (field) =>
		field.topicDCV + '_' + field.fieldSetId + '_' + field.fieldId
});

const initialFieldState: FieldState = fieldsAdapter.getInitialState({
	allFieldsLoaded: false
});

const mapUpdateField = (payload: Field): Update<Field> => {
	const { topicDCV, fieldSetId, fieldId, data } = payload;
	return {
		id: `${topicDCV}_${fieldSetId}_${fieldId}`,
		changes: {
			data
		}
	};
};

export const reducer = createReducer(
	initialFieldState,
	on(
		fieldActions.LoadFields,
		(state): FieldState => ({
			...state,
			allFieldsLoaded: false
		})
	),
	on(
		fieldActions.UpdateFieldSuccess,
		fieldActions.UpdateFieldsSuccess,
		fieldActions.LoadFieldsFail,
		(state): FieldState => ({
			...state,
			allFieldsLoaded: true
		})
	),
	on(fieldActions.UpdateField, (state, { payload }) =>
		fieldsAdapter.updateOne(mapUpdateField(payload), {
			...state,
			allFieldsLoaded: false
		})
	),
	on(fieldActions.UpdateFieldFail, (state, { payload }) =>
		fieldsAdapter.updateOne(mapUpdateField(payload), {
			...state,
			allFieldsLoaded: true
		})
	),
	on(fieldActions.UpdateFields, (state, { payload }) =>
		fieldsAdapter.updateMany(payload.map(mapUpdateField), {
			...state,
			allFieldsLoaded: false
		})
	),
	on(fieldActions.UpdateFieldsFail, (state, { payload }) =>
		fieldsAdapter.updateMany(payload.map(mapUpdateField), {
			...state,
			allFieldsLoaded: true
		})
	),
	on(fieldActions.LoadFieldsSuccess, (state, { payload }) =>
		fieldsAdapter.addMany(payload, { ...state, allFieldsLoaded: true })
	),
	on(
		languageActions.UpdateLanguage,
		(): FieldState => ({ ...initialFieldState })
	)
);

export const {
	selectAll: selectAllFields,
	selectEntities: selectFieldEntities,
	selectIds: selectFieldIds,
	selectTotal: fieldsCount
} = fieldsAdapter.getSelectors();
