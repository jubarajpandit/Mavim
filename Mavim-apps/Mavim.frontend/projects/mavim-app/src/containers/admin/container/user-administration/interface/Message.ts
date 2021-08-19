import { MessageType } from '../enums/MessageType';

export interface Message {
	text: string;
	type: MessageType;
}
