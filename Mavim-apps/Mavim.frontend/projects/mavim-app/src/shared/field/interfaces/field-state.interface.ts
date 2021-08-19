import { Field } from '../models/field.model';
import { EntityState } from '@ngrx/entity';

export interface FieldState extends EntityState<Field> {
	allFieldsLoaded: boolean;
}
