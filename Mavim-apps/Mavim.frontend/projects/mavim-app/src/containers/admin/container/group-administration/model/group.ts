import { Role } from './Role';

export interface Group {
	id: string;
	title: string;
	description: string;
	roles: Role[];
}
