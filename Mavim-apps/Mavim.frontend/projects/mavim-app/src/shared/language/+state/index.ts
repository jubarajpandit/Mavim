import { Language } from '../enums/language.enum';

export * from './language.reducer';
export * from './language.actions';

export class LanguageState {
	public databaseLanguage: Language;
}
