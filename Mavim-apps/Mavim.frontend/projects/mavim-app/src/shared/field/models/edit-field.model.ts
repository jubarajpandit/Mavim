import { Field } from './field.model';
import { EditStatus } from '../../../containers/edit/enums/edit-status.enum';

export class EditField extends Field {
	public status: EditStatus;
}
