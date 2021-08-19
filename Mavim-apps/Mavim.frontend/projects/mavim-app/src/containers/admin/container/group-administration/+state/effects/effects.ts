import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { GroupService } from '../../service/group.service';
import * as GroupManagementActions from '../actions/actions';
@Injectable({
	providedIn: 'root'
})
export class GroupEffects {
	public constructor(
		private readonly actions$: Actions,
		private readonly service: GroupService
	) {}

	public actionName$ = createEffect(() => {
		return this.actions$.pipe(
			ofType(GroupManagementActions.loadGroups),
			mergeMap(() =>
				this.service.getGroups().pipe(
					map((groups) =>
						GroupManagementActions.loadGroupsSuccess({
							payload: groups
						})
					),
					catchError(() =>
						of(GroupManagementActions.loadGroupsFailed())
					)
				)
			)
		);
	});
}
