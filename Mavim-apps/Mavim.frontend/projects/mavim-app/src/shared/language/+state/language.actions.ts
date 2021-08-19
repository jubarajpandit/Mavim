import { Language } from '../enums/language.enum';
import { createAction, props } from '@ngrx/store';

export const InitialLanguage = createAction(
	'[Language] Initial Language',
	props<{ payload: Language }>()
);
export const UpdateLanguage = createAction(
	'[Language] Update Language',
	props<{ payload: Language }>()
);
