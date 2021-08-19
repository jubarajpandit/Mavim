import { Injectable } from '@angular/core';
import { Actions, ofType, createEffect } from '@ngrx/effects';
import * as wopiActions from '../actions/wopi.actions';
import { mergeMap, map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { WopiService } from '../../services/wopi.service';
import { Wopi } from '../../models/wopi.model';
import { WopiActionUrls } from '../../models/wopi-actionurls.model';

@Injectable()
export class WopiEffects {
	public constructor(
		private readonly actions$: Actions,
		private readonly wopiService: WopiService
	) {}

	public loadFields$ = createEffect(() => {
		return this.actions$.pipe(
			ofType(wopiActions.WopiActionTypes.LoadFileInfo),
			mergeMap((action: wopiActions.LoadFileInfo) =>
				this.wopiService.getFileInfo(action.payload).pipe(
					map(
						(wopi: Wopi) =>
							new wopiActions.LoadFileInfoSuccess(wopi)
					),
					catchError(() =>
						of(
							new wopiActions.LoadFileInfoFail(
								this.getDefaultDescriptionWopi(action.payload)
							)
						)
					)
				)
			)
		);
	});

	public loadWopiActionUrls$ = createEffect(() => {
		return this.actions$.pipe(
			ofType(wopiActions.WopiActionTypes.LoadWopiActionUrls),
			mergeMap(() =>
				this.wopiService.getWopiActionUrls().pipe(
					map(
						(wopiActionUrls: WopiActionUrls) =>
							new wopiActions.LoadWopiActionUrlsSuccess(
								wopiActionUrls
							)
					),
					catchError(() =>
						of(new wopiActions.LoadWopiActionUrlsFail(undefined))
					)
				)
			)
		);
	});

	private getDefaultDescriptionWopi(dcv: string): Wopi {
		const defaultWopi: Wopi = {
			topicDCV: dcv,
			hasDescription: false,
			descriptionLoaded: false
		};

		return defaultWopi;
	}
}
