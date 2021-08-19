import { RelationTopic } from '../../../../relation/models/relation-topic.model';
import { APIFieldValueType } from '../apifieldvaluetype';

export abstract class ApiField {
	public fieldSetId: string;
	public fieldId: string;
	public topicDCV: string;
	public setName: string;
	public fieldName: string;
	public fieldValueType: APIFieldValueType;
	public isMultiValue: boolean;
	public required: boolean;
	public readonly: boolean;
	public usage: string;
	public relationshipCategory: RelationTopic;
	public characteristic: RelationTopic;
	public openLocation: string;
}
