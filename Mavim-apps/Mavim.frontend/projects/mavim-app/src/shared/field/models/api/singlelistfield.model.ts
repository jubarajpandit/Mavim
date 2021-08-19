import { SingleField } from './abstract/singlefield.model';
import { Dictionary } from '@ngrx/entity';

export class SingleListField extends SingleField<Dictionary<string>> {
	public options: Dictionary<string>;
}
