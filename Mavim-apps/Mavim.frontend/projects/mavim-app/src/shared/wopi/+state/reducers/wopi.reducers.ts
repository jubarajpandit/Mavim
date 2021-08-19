import {
	createEntityAdapter,
	EntityAdapter,
	EntityState,
	Update
} from '@ngrx/entity';
import { Wopi } from '../../models/wopi.model';
import { WopiActions, WopiActionTypes } from '../actions/wopi.actions';
import { WopiActionUrlsState } from '../../interfaces/wopiactionurlsstate';
import { FetchStatus } from '../../../enums/FetchState';

const wopiAdapter: EntityAdapter<Wopi> = createEntityAdapter<Wopi>({
	selectId: (wopi) => wopi.topicDCV
});

const initialWopiState: EntityState<Wopi> = wopiAdapter.getInitialState();

export const initialDatabaseState: WopiActionUrlsState = {
	wopiActionUrls: undefined,
	fetchWopiActionUrls: FetchStatus.NotFetched
};

export function wopiReducer(
	state: EntityState<Wopi> = initialWopiState,
	action: WopiActions
): EntityState<Wopi> {
	switch (action.type) {
		case WopiActionTypes.LoadFileInfo:
			return wopiAdapter.upsertOne(
				getInitialDescription(action.payload),
				{ ...state }
			);
		case WopiActionTypes.LoadFileInfoFail:
		case WopiActionTypes.LoadFileInfoSuccess:
			return wopiAdapter.updateOne(
				getUpdatedDescription(action.payload),
				{ ...state }
			);
		default:
			return state;
	}
}

export function wopiActionUrlsReducer(
	state: WopiActionUrlsState = initialDatabaseState,
	action: WopiActions
): WopiActionUrlsState {
	switch (action.type) {
		case WopiActionTypes.LoadWopiActionUrls:
			return { ...state, fetchWopiActionUrls: FetchStatus.Loading };
		case WopiActionTypes.LoadWopiActionUrlsFail:
			return { ...state, fetchWopiActionUrls: FetchStatus.Fetched };
		case WopiActionTypes.LoadWopiActionUrlsSuccess:
			return {
				...state,
				wopiActionUrls: action.payload,
				fetchWopiActionUrls: FetchStatus.Fetched
			};
		default:
			return state;
	}
}

function getUpdatedDescription(wopi: Wopi): Update<Wopi> {
	const updatedWopi = {
		id: wopi.topicDCV,
		changes: {
			hasDescription: wopi.hasDescription,
			descriptionLoaded: true
		}
	};

	return updatedWopi;
}

function getInitialDescription(dcv: string): Wopi {
	const initialWopi: Wopi = {
		topicDCV: dcv,
		hasDescription: false,
		descriptionLoaded: false
	};

	return initialWopi;
}
