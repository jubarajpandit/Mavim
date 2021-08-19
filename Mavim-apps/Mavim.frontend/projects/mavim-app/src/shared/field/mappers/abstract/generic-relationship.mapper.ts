import { GenericFieldMapper } from './generic-field.mapper';
import { RelationTopic } from '../../../relation/models/relation-topic.model';

export abstract class GenericRelationshipFieldMapper<
	T
> extends GenericFieldMapper<T> {
	private readonly numberOfRelationshipItems = 3;

	protected mapToString(relationship: RelationTopic): string {
		if (!relationship) {
			return undefined;
		}

		return `${relationship.dcv}:${relationship.name}:${relationship.icon}`;
	}

	protected mapToRelationship(relationship: string): RelationTopic {
		if (!relationship) {
			return undefined;
		}
		const [dcv, name, icon] = relationship.split(
			':',
			this.numberOfRelationshipItems
		);

		return dcv && name
			? ({
					dcv,
					name,
					icon
			  } as RelationTopic)
			: undefined;
	}
}
