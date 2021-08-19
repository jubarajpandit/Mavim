import { createFeatureSelector, createSelector } from '@ngrx/store';
import { DatabaseState } from '../../interfaces/database-state.interface';

export const selectDatabaseState =
	createFeatureSelector<DatabaseState>('database');

export const selectSelectedDatabases = createSelector(
	selectDatabaseState,
	(databaseState: DatabaseState) => databaseState.selectedDatabase
);

export const selectDatabases = createSelector(
	selectDatabaseState,
	(databaseState: DatabaseState) => databaseState.databases
);

export const selectFetchDatabase = createSelector(
	selectDatabaseState,
	(databaseState: DatabaseState) => databaseState.fetchDatabase
);
