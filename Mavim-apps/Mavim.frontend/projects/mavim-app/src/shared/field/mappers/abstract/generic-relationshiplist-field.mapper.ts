import { GenericFieldMapper } from './generic-field.mapper';
import { RelationTopic } from '../../../relation/models/relation-topic.model';
import { Dictionary } from '@ngrx/entity';

export abstract class GenericRelationshiplistFieldMapper<
	T
> extends GenericFieldMapper<T> {
	private readonly numberOfRelationshipItems = 2;

	protected mapToString(
		relationshipList: Dictionary<RelationTopic>
	): string[] {
		if (!relationshipList) {
			return [];
		}

		return Object.keys(relationshipList).map(
			(key) => `${key}:${relationshipList[key].name}`
		);
	}

	protected mapToOptionsString(
		relationships: Dictionary<RelationTopic>
	): Dictionary<string> {
		if (!relationships) {
			return undefined;
		}

		const dictionary: Dictionary<string> = {};

		Object.keys(relationships).forEach((key) => {
			const { dcv, name, icon } = relationships[key];
			dictionary[key] = `${dcv}:${name}:${icon}`;
		});

		return dictionary;
	}

	protected mapToApiModel(
		relationships: string[]
	): Dictionary<RelationTopic> {
		if (!relationships) {
			return undefined;
		}

		const dictionary: Dictionary<RelationTopic> = {};

		relationships.forEach((relation) => {
			const data: string[] = relation.split(
				':',
				this.numberOfRelationshipItems
			);
			dictionary[data[0]] = {
				dcv: data[0],
				name: data[1]
			} as RelationTopic;
		});

		return dictionary;
	}
}
