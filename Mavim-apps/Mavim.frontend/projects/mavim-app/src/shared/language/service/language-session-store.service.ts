import { Injectable } from '@angular/core';
import { Language } from '../enums/language.enum';

@Injectable({ providedIn: 'root' })
export class LanguageSessionStoreService {
	public constructor() {
		this.storage = localStorage;
	}
	private readonly databaseLanguageSessionStoreKey = 'MAVDBLANGUAGE';
	private readonly storage: Storage;

	/**
	 * Returns the database language from the session store
	 */

	public get getDatabaseLanguage(): string {
		return this.storage.getItem(this.databaseLanguageSessionStoreKey);
	}

	/**
	 * Sets the database language in the sessions store
	 */
	public setDatabaseLanguage(language: Language): void {
		this.storage.setItem(this.databaseLanguageSessionStoreKey, language);
	}
}
