import { Topic } from '../../topic/models/topic.model';
import { DispatchInstruction } from './dispatchInstruction.model';
import { RelationTopic } from './relation-topic.model';

export class Relation {
	public dcv: string;
	public isTypeOfTopic: boolean;
	public topicDCV: string;
	public category: string;
	public categoryType: string;
	public icon: string;
	public userInstruction: Topic;
	public dispatchInstructions: DispatchInstruction[];
	public characteristic: RelationTopic;
	public withElement: RelationTopic;
	public withElementParent?: RelationTopic;
}
