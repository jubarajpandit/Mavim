import {
	Component,
	Input,
	EventEmitter,
	Output,
	ChangeDetectionStrategy
} from '@angular/core';
import { Field } from '../../models/field.model';
import { FieldValueType } from '../../models/fieldvaluetype.enum';
import * as moments from 'moment';
import { FieldBaseDirective } from '../shared/fieldbase';

@Component({
	selector: 'mav-fields',
	templateUrl: './fields.component.html',
	styleUrls: ['./fields.component.scss'],
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class FieldsComponent extends FieldBaseDirective {
	@Input() public fields: Field[];
	@Output() public internalDcvId = new EventEmitter<string>();

	public fieldType = FieldValueType;
	private readonly language = 'nl'; // TODO: Traslate service (WorkItem: 14169)

	public get fieldSets(): string[] {
		return [...new Set(this.fields.map((p) => p.setName))];
	}

	public getFieldsOfFieldSet(fieldSetName: string): Field[] {
		return this.fields.filter((field) => field.setName === fieldSetName);
	}

	public renderCellValueContent(
		fieldValue: string | null,
		fieldValueTypeText: string
	): string {
		if (fieldValue === null) {
			return '';
		}
		const fieldValueType = FieldValueType[
			fieldValueTypeText
		] as FieldValueType;
		switch (fieldValueType) {
			case FieldValueType.relationshipList:
			case FieldValueType.relationship:
			case FieldValueType.multiRelationship:
			case FieldValueType.list: {
				return this.getNameFromFieldValue(fieldValue);
			}
			case FieldValueType.boolean: {
				return this.booleanFormat(fieldValue);
			}
			case FieldValueType.date:
			case FieldValueType.multiDate: {
				return this.dateFormat(fieldValue);
			}
			case FieldValueType.number:
			case FieldValueType.multiNumber:
			case FieldValueType.text:
			case FieldValueType.multiText:
			case FieldValueType.decimal:
			case FieldValueType.multiDecimal:
			case FieldValueType.hyperlink:
			case FieldValueType.multiHyperlink:
				return fieldValue;
			default: {
				return '';
			}
		}
	}

	public emitInternalLinkEvent(dcvId: string): void {
		if (dcvId) {
			this.internalDcvId.emit(dcvId);
		}
	}

	public getFieldValueType(fieldValueType: string): FieldValueType {
		return FieldValueType[fieldValueType] as FieldValueType;
	}

	private booleanFormat(fieldValue: string): string {
		return !fieldValue ||
			fieldValue.localeCompare('false', undefined, {
				sensitivity: 'base'
			}) === 0
			? this.booleanFalseValue
			: this.booleanTrueValue;
	}

	private dateFormat(fieldValue: string): string {
		return moments(fieldValue).isValid()
			? moments(fieldValue).locale(this.language).format('LL')
			: '';
	}
}
