import { Action } from '@ngrx/store';
import { Database } from '../../models/database';

export enum DatabaseActionTypes {
	InitDatabase = '[Database] Initialise Database State',
	LoadDatabase = '[Database] Load Database',
	LoadDatabaseSuccess = '[Database] Load Database Success',
	LoadDatabaseFailed = '[Database] Load Database Failed',
	SetSelectedDatabase = '[Database], Set Selected Database'
}

// Action Creators
export class InitDatabase implements Action {
	public readonly type = DatabaseActionTypes.InitDatabase;
}
export class LoadDatabase implements Action {
	public readonly type = DatabaseActionTypes.LoadDatabase;
}
export class LoadDatabaseSuccess implements Action {
	public constructor(public database: Database[]) {}
	public readonly type = DatabaseActionTypes.LoadDatabaseSuccess;
}
export class LoadDatabaseFailed implements Action {
	public readonly type = DatabaseActionTypes.LoadDatabaseFailed;
}

export class SetSelectedDatabase implements Action {
	public constructor(public database: string) {}
	public readonly type = DatabaseActionTypes.SetSelectedDatabase;
}

export type DatabaseActions =
	| InitDatabase
	| LoadDatabase
	| LoadDatabaseSuccess
	| LoadDatabaseFailed
	| SetSelectedDatabase;
