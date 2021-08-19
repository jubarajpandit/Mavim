import { createReducer, on } from '@ngrx/store';
import { LanguageState } from '.';
import * as LanguageActions from './language.actions';

const initialLanguageState: LanguageState = {
	databaseLanguage: undefined
};

export const languageReducer = createReducer(
	initialLanguageState,
	on(
		LanguageActions.InitialLanguage,
		LanguageActions.UpdateLanguage,
		(state, action): LanguageState => ({
			...state,
			databaseLanguage: action.payload
		})
	)
);
