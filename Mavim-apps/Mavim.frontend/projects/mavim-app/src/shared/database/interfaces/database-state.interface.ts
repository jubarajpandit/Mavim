import { Database } from '../models/database';
import { FetchStatus } from '../../enums/FetchState';

export interface DatabaseState {
	selectedDatabase: string;
	databases: Database[];
	fetchDatabase: FetchStatus;
}
