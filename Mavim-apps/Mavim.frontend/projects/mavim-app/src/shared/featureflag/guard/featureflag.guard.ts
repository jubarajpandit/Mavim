import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { filter, mergeMap } from 'rxjs/operators';
import { FetchStatus } from '../../enums/FetchState';
import { FeatureflagFacade } from '../service/featureflag.facade';

@Injectable({
	providedIn: 'root'
})
export class FeatureflagGuard implements CanActivate {
	public constructor(private readonly featureFlagFacade: FeatureflagFacade) {}

	public canActivate(
		route: ActivatedRouteSnapshot
	): Observable<boolean> | Promise<boolean> | boolean {
		const featureflag = route.data.featureflag as string;

		return this.featureFlagFacade.fetchFeatureflags.pipe(
			filter((status) => status === FetchStatus.Fetched),
			mergeMap(() => this.featureFlagFacade.getFeatureflag(featureflag))
		);
	}
}
