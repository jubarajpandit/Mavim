import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import { FieldService } from '../../services/field.service';
import * as fieldActions from '../actions/field.actions';
import { mergeMap, map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { Field } from '../../models/field.model';

@Injectable()
export class FieldEffects {
	public constructor(
		private readonly actions$: Actions,
		private readonly fieldService: FieldService
	) {}

	public loadFields$ = createEffect(() => {
		return this.actions$.pipe(
			ofType(fieldActions.LoadFields),
			mergeMap((action) =>
				this.fieldService.getFieldSets(action.payload).pipe(
					map((fields: Field[]) =>
						fieldActions.LoadFieldsSuccess({ payload: fields })
					),
					catchError(() => of(fieldActions.LoadFieldsFail()))
				)
			)
		);
	});
}
