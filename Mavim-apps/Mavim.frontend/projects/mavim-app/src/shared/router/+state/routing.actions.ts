import { createAction, props } from '@ngrx/store';
import { Topic } from '../../topic/models/topic.model';
import { Outlet } from '../models/outlet';

export const Init = createAction(
	'[Routing] Init queue',
	props<{ payload: string[] }>()
);
export const Home = createAction(
	'[Routing] Go to homepage',
	props<{ payload: string }>()
);
export const Next = createAction(
	'[Routing] Next page',
	props<{ payload: { dcvId: string; outlet: Outlet } }>()
);
export const Back = createAction('[Routing] Navigate back');
export const Edit = createAction(
	'[Routing] Go to Editpage',
	props<{ payload: string }>()
);
export const EditWord = createAction(
	'[Routing] Go to Word edit page',
	props<{ payload: string }>()
);
export const TestWopi = createAction(
	'[Routing] Go to test Wopi page',
	props<{ payload: string }>()
);
export const CreateNewWord = createAction(
	'[Routing] Go to create new Word page',
	props<{ payload: Topic }>()
);
export const EditUsers = createAction('[Routing] Go to Users edit page');
export const BackEdit = createAction('[Routing] Return from Edit');
export const UpdateQueue = createAction(
	'[Routing] Update Queue',
	props<{ payload: string[] }>()
);
