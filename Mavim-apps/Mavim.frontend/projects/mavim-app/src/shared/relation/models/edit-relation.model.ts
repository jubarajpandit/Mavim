import { Relation } from './relation.model';
import { EditStatus } from '../../../containers/edit/enums/edit-status.enum';

export class EditRelation extends Relation {
	public status: EditStatus;
}
