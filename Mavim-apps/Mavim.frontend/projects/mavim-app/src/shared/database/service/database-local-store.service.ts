import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable()
export class DatabaseBrowserService {
	public constructor() {
		this.storage = localStorage;
	}
	private readonly localStoreKey = 'database';
	private readonly storage: Storage;

	/**
	 * Returns the dvc queue from the session store
	 */
	public get getSelectedDatabase(): Observable<string> {
		const json = this.storage.getItem(this.localStoreKey);
		const dbId = JSON.parse(json) as string;
		return of(dbId);
	}

	/**
	 * Sets the session store key for the array of dcv ids
	 */
	public setQueue(dbId: string): void {
		const initdbId = JSON.stringify(dbId);
		this.storage.setItem(this.localStoreKey, initdbId);
	}
}
