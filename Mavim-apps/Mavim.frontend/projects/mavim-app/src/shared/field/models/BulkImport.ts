import { Failed } from './failed';

export class BulkImport<T> {
	public succeeded: T[];
	public failed: Failed<T>[];
}
