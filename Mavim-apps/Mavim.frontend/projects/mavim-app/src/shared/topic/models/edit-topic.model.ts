import { Topic } from './topic.model';
import { EditStatus } from '../../../containers/edit/enums/edit-status.enum';

export class EditTopic extends Topic {
	public editStatus: EditStatus;
}
