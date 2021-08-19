import { createAction, props } from '@ngrx/store';
import { Group } from '../../model/group';

export const loadGroups = createAction('[groups] Load Groups');

export const loadGroupsSuccess = createAction(
	'[groups] Load Groups Success',
	props<{ payload: Group[] }>()
);

export const loadGroupsFailed = createAction('[groups] Load Groups Failure');
