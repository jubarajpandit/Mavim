import { FieldValueType } from './fieldvaluetype.enum';
import { RelationTopic } from '../../relation/models/relation-topic.model';
import { Dictionary } from '@ngrx/entity';

export class Field {
	public fieldSetId: string;
	public fieldId: string;
	public setOrder: number;
	public order: number;
	public topicDCV: string;
	public setName: string;
	public fieldName: string;
	public fieldValueType: FieldValueType;
	public data: string[];
	public isMultiValue: boolean;
	public required: boolean;
	public readonly: boolean;
	public usage: string;
	public options?: Dictionary<string>;
	public relationshipCategory: RelationTopic;
	public characteristic: RelationTopic;
	public openLocation: string;
}
