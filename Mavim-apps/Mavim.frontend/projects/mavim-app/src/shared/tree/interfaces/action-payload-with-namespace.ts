import { Action } from '@ngrx/store';
import { ActionNameSpace } from './action-namespace';

export interface ActionPayloadWithNameSpace extends Action {
	payload: ActionNameSpace;
}
