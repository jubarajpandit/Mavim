import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import * as actions from '../+state/language.actions';
import { selectCurrentLanguage } from '../+state/language.selectors';
import { Observable } from 'rxjs';
import { Language } from '../enums/language.enum';

@Injectable({ providedIn: 'root' })
export class LanguageFacade {
	public constructor(private readonly store: Store) {}

	/**
	 * Gets the language from the store
	 */
	public get language(): Observable<Language> {
		return this.store.select(selectCurrentLanguage);
	}

	/**
	 * Sets the initial language in the store
	 */
	public initialLanguage(language: Language): void {
		this.store.dispatch(actions.InitialLanguage({ payload: language }));
	}

	/**
	 * Updates the language in the store
	 */
	public updateLanguage(language: Language): void {
		this.store.dispatch(actions.UpdateLanguage({ payload: language }));
	}
}
