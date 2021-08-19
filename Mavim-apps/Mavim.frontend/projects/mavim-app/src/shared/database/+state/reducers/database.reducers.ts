import { DatabaseState } from '../../interfaces/database-state.interface';
import {
	DatabaseActions,
	DatabaseActionTypes
} from '../actions/database.actions';
import { FetchStatus } from '../../../enums/FetchState';

export const initialDatabaseState: DatabaseState = {
	selectedDatabase: undefined,
	databases: undefined,
	fetchDatabase: FetchStatus.NotFetched
};

export function databaseReducer(
	state = initialDatabaseState,
	action: DatabaseActions
): DatabaseState {
	switch (action.type) {
		case DatabaseActionTypes.LoadDatabase:
			return { ...state, fetchDatabase: FetchStatus.Loading };
		case DatabaseActionTypes.LoadDatabaseFailed:
			return { ...state, fetchDatabase: FetchStatus.Fetched };
		case DatabaseActionTypes.LoadDatabaseSuccess: {
			const selectedDatabase: string =
				action.database && action.database.length > 0
					? action.database[0].id
					: undefined;
			return {
				...state,
				fetchDatabase: FetchStatus.Fetched,
				databases: action.database,
				selectedDatabase
			};
		}
		case DatabaseActionTypes.SetSelectedDatabase: {
			return { ...state, selectedDatabase: action.database };
		}
		default:
			return state;
	}
}
