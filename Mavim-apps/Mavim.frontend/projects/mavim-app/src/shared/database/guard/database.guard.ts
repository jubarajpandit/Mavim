import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { DatabaseFacade } from '../service/database.facade';
import { filter, map, tap, mergeMap } from 'rxjs/operators';
import { FetchStatus } from '../../enums/FetchState';

@Injectable({ providedIn: 'root' })
export class DatabaseGuard implements CanActivate {
	public constructor(
		private readonly router: Router,
		private readonly databaseFacade: DatabaseFacade
	) {}

	public canActivate(): Observable<boolean> {
		return this.databaseFacade.fetchDatabase.pipe(
			filter((status) => status === FetchStatus.Fetched),
			mergeMap(() =>
				this.databaseFacade.selectedDatabase.pipe(
					map((database) => !!database),
					tap((database) => {
						if (!database) {
							this.router.navigate(['/']);
						}
					})
				)
			)
		);
	}
}
