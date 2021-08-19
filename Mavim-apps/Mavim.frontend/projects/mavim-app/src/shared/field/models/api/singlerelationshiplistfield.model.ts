import { SingleField } from './abstract/singlefield.model';
import { RelationTopic } from '../../../relation/models/relation-topic.model';
import { Dictionary } from '@ngrx/entity';

export class SingleRelationshipListField extends SingleField<
	Dictionary<RelationTopic>
> {
	public options: Dictionary<RelationTopic>;
}
