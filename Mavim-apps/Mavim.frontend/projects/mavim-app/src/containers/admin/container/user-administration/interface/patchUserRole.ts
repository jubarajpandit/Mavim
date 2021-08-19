import { Role } from '../../../../../shared/authorization/enums/role';

export interface PatchUserRole {
	id: string;
	role: Role;
}
