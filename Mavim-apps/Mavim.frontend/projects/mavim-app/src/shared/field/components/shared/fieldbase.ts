import { Directive } from '@angular/core';
import { RegexUtils } from '../../../utils';

@Directive()
export abstract class FieldBaseDirective {
	public readonly booleanFalseValue = 'No';
	public readonly booleanTrueValue = 'Yes';
	private readonly numberOfRelationshipItems = 3;

	public getDcvIdFromFieldValue(fieldValue: string | null): string {
		return fieldValue?.split(':')[0];
	}

	public getNameFromFieldValue(fieldValue: string): string {
		if (this.isEmpty(fieldValue)) {
			return '<empty>';
		}

		const [, name] = fieldValue.split(':', this.numberOfRelationshipItems);
		return name;
	}

	public getIconFromFieldValue(fieldValue: string): string {
		if (this.isEmpty(fieldValue)) {
			return undefined;
		}

		const [, , icon] = fieldValue.split(
			':',
			this.numberOfRelationshipItems
		);
		return icon;
	}

	public getSelectedListKey(fieldValue: string): string {
		if (this.isEmpty(fieldValue)) {
			return undefined;
		}

		const [dcv] = fieldValue.split(':', this.numberOfRelationshipItems);
		return dcv;
	}

	public isEmpty(text: string): boolean {
		return (
			text === undefined || RegexUtils.whitespace().exec(text)?.length > 0
		);
	}
}
