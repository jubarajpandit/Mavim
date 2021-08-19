import { Role } from '../../../../../shared/authorization/enums/role';

export interface AddUserRole {
	email: string;
	role: Role;
}
