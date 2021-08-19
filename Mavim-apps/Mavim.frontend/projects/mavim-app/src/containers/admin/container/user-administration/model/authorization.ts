import { Role } from '../../../../../shared/authorization/enums/role';

export interface User {
	id: string;
	email: string;
	tenantId: string;
	role: Role;
}
