import {
	createFeatureSelector,
	createSelector,
	DefaultProjectorFn,
	MemoizedSelector
} from '@ngrx/store';
import { Wopi } from '../../models/wopi.model';
import { EntityState } from '@ngrx/entity';
import { WopiActionUrlsState } from '../../interfaces/wopiactionurlsstate';

export const selectWopiState = createFeatureSelector<EntityState<Wopi>>('wopi');

export const selectWopiActionUrlState =
	createFeatureSelector<WopiActionUrlsState>('wopiactionurls');

export const selectWopiByDcv = (
	dcv: string
): MemoizedSelector<EntityState<Wopi>, Wopi, DefaultProjectorFn<Wopi>> =>
	createSelector(selectWopiState, (wopiState) => {
		return wopiState.entities[dcv];
	});

export const selectWopiActionUrls = createSelector(
	selectWopiActionUrlState,
	(wopiAcionUrlState: WopiActionUrlsState) => wopiAcionUrlState.wopiActionUrls
);

export const selectFetchWopiActionUrls = createSelector(
	selectWopiActionUrlState,
	(wopiAcionUrlState: WopiActionUrlsState) =>
		wopiAcionUrlState.fetchWopiActionUrls
);
