import { createFeatureSelector, createSelector } from '@ngrx/store';
import { LanguageState } from '.';

export const selectLanguageState =
	createFeatureSelector<LanguageState>('language');

export const selectCurrentLanguage = createSelector(
	selectLanguageState,
	(languageState) => languageState.databaseLanguage
);
