import { Role } from '../enums/role';

export class Authorization {
	public id: string;
	public email: string;
	public tenantId: string;
	public role: Role;
}
