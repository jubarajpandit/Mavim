import {
	Component,
	Input,
	EventEmitter,
	Output,
	ChangeDetectionStrategy
} from '@angular/core';
import { FormValidationError } from '../../../../containers/edit/models/formvalidationerror.model';
import { EditField } from '../../models/edit-field.model';
import { EditStatus } from '../../../../containers/edit/enums/edit-status.enum';
import { FieldBaseDirective } from '../shared/fieldbase';
import { Field } from '../../models/field.model';
import { Dictionary } from '@ngrx/entity';
import { SelectBox } from '../../models';
import { GenericDecimalFieldMapper } from '../../mappers/abstract/generic-decimal.field.mapper';
import { Topic } from '../../../topic/models/topic.model';
import { FieldValueType } from '../../models/fieldvaluetype.enum';
import { RegexUtils } from '../../../utils/regex.utils';

@Component({
	selector: 'mav-edit-fields',
	templateUrl: './fields-edit.component.html',
	styleUrls: ['./fields-edit.component.scss'],
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class FieldsEditComponent extends FieldBaseDirective {
	@Output() public fieldsChanged = new EventEmitter<EditField[]>();
	@Input() public fields: EditField[];
	@Output() public validationError = new EventEmitter<FormValidationError>();
	public listBoxChoice = 'Make your choice';

	public showModal = false;
	public modalOpenLocation: string;
	public fieldType = FieldValueType;

	private relationField: Field;
	private fieldValueIndex: number;

	public get fieldSets(): string[] {
		return [...new Set(this.fields.map((p) => p.setName))];
	}

	public getFieldsOfFieldSet(fieldSetName: string): Field[] {
		return this.fields.filter((field) => field.setName === fieldSetName);
	}

	public getFieldValueType(fieldValueType: string): FieldValueType {
		return FieldValueType[fieldValueType] as FieldValueType;
	}

	public onDateChangeEvent(
		changedField: Field,
		changeEvent: HTMLInputElement,
		index: number
	): void {
		changeEvent.blur();
		const newValue = changeEvent.valueAsDate;
		if (newValue) {
			this.updateChangedFieldsAndEmit(
				changedField,
				index,
				newValue.toISOString()
			);
		}
	}

	public clearFieldValue(changedField: Field, index: number): void {
		const newValue = '';
		this.updateChangedFieldsAndEmit(changedField, index, newValue);
	}

	public onHyperlinkChangedEvent(
		changedField: Field,
		changeEvent: HTMLInputElement
	): void {
		const fieldValueIndex = Number(changeEvent.name);
		const newValue = changeEvent.value;

		if (newValue === '' || RegexUtils.hyperlink().test(newValue)) {
			this.updateChangedFieldsAndEmit(
				changedField,
				fieldValueIndex,
				newValue
			);
		} else {
			const formValidationError: FormValidationError = {
				componentName: 'HyperlinkField ',
				errorType: 'hyperlink not in correct format'
			};
			this.validationError.emit(formValidationError);
		}
	}

	public onRelationChangedEvent(changedField: Field, index: number): void {
		this.relationField = changedField;
		this.fieldValueIndex = index;
		this.showModal = true;
		this.modalOpenLocation =
			changedField?.data[index]?.split(':', 1)[0] ??
			changedField.openLocation;
	}

	public hideFieldRelationsModal(): void {
		this.relationField = undefined;
		this.fieldValueIndex = undefined;
		this.showModal = false;
	}

	public saveRelationField(topic: Topic): void {
		const newValue = `${topic.dcv}:${topic.name}:${topic.icon}`;
		this.updateChangedFieldsAndEmit(
			this.relationField,
			this.fieldValueIndex,
			newValue,
			true
		);
		this.hideFieldRelationsModal();
	}

	public replaceCommaWithDot(value: string): string {
		return value?.replace(',', '.');
	}

	public onDecimalChangeEvent(
		changedField: Field,
		changeEvent: HTMLInputElement
	): void {
		const fieldValueIndex = Number(changeEvent.name);
		const changedValue: string = changeEvent.value;
		const decimal: number = GenericDecimalFieldMapper.parseDecimal(
			changeEvent.value
		);

		if (this.isEmpty(changedValue) || decimal) {
			this.updateChangedFieldsAndEmit(
				changedField,
				fieldValueIndex,
				changedValue
			);
		} else {
			const formValidationError: FormValidationError = {
				componentName: 'fields',
				errorType: 'decimal not in correct format'
			};
			this.validationError.emit(formValidationError);
		}
	}

	public onListValueChanged(changedField: Field, key: string): void {
		this.updateChangedFieldsAndEmit(
			changedField,
			0,
			changedField.options[key]
		);
	}

	public onFieldChangedEvent(
		changedField: Field,
		changeEvent: HTMLInputElement
	): void {
		const fieldValueIndex = Number(changeEvent.name);
		const newValue = changeEvent.value;

		this.updateChangedFieldsAndEmit(
			changedField,
			fieldValueIndex,
			newValue
		);
	}

	public onToggleEvent(
		changedField: Field,
		changeValue: HTMLInputElement,
		index: number
	): void {
		this.updateChangedFieldsAndEmit(
			changedField,
			index,
			changeValue.checked.toString()
		);
	}

	public parseBooleanString(booleanValue: string): boolean {
		return booleanValue === undefined
			? false
			: booleanValue.localeCompare('true', undefined, {
					sensitivity: 'base'
			  }) === 0;
	}

	public dictionaryRelationshipToArray(
		value: Dictionary<string>
	): SelectBox[] {
		const relationSplit = 3;

		const array: SelectBox[] = value
			? Object.keys(value).map((propName): SelectBox => {
					const [key, text, icon] = value[propName].split(
						':',
						relationSplit
					);
					return { key, text, icon } as SelectBox;
			  })
			: [];
		return array;
	}

	public dictionaryToArray(value: Dictionary<string>): SelectBox[] {
		const listSplit = 3;
		const array: SelectBox[] = value
			? Object.keys(value).map((propName): SelectBox => {
					const [key, text] = value[propName].split(':', listSplit);
					return { key, text } as SelectBox;
			  })
			: [];
		return array;
	}

	public getDataFromField(field: Field): string[] {
		return field?.data ?? [''];
	}

	public getDataFromStringArray(data: string[]): string[] {
		return data?.length > 0 ? data : [''];
	}

	private updateChangedFieldsAndEmit(
		changedField: Field,
		fieldValueIndex: number,
		newValue: string,
		updateFieldSets = false
	): void {
		const updatedField = this.fields.find(
			(field: Field) =>
				field.fieldSetId === changedField.fieldSetId &&
				field.fieldId === changedField.fieldId
		);

		const data = [...updatedField.data];
		data[fieldValueIndex] = newValue;
		updatedField.data = data;
		updatedField.status = EditStatus.Updated;

		if (updateFieldSets) {
			// eslint-disable-next-line no-self-assign
			this.fields = this.fields;
		}
		this.fieldsChanged.emit(this.fields);
	}
}
