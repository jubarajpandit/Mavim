import { Field } from '../../../../shared/field/models/field.model';

export interface IEditPanelFacade {
	updateFieldValues(newFieldValue: Field): void;
	updateFieldsValues(fields: Field[]): void;
}
