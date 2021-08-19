import { User } from '../model/authorization';
import { Message } from './Message';

export interface AdminUserState {
	users: User[];
	usersLoaded: boolean;
	message: Message;
}
