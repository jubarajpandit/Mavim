import { createAction, props } from '@ngrx/store';

export const MoveToTop = createAction(
	'[Tree Edit] Move Topic To First Position In Branch',
	props<{ payload: { namespace: string; topicId: string } }>()
);
export const MoveToBottom = createAction(
	'[Tree Edit] Move Topic To Last Position In Branch',
	props<{ payload: { namespace: string; topicId: string } }>()
);
export const MoveUp = createAction(
	'[Tree Edit] Move Topic One Position Up',
	props<{ payload: { namespace: string; topicId: string } }>()
);
export const MoveDown = createAction(
	'[Tree Edit] Move Topic One Position Down',
	props<{ payload: { namespace: string; topicId: string } }>()
);
export const MoveLevelUp = createAction(
	'[Tree Edit] Move Topic One Level Up',
	props<{ payload: { namespace: string; topicId: string } }>()
);
export const MoveLevelDown = createAction(
	'[Tree Edit] Move Topic One Level Down',
	props<{ payload: { namespace: string; topicId: string } }>()
);

export const MoveToTopSuccess = createAction(
	'[Tree Edit] Move Topic To First Position In Branch Success',
	props<{ payload: { namespace: string; topicId: string } }>()
);
export const MoveToBottomSuccess = createAction(
	'[Tree Edit] Move Topic To Last Position In Branch Success',
	props<{ payload: { namespace: string; topicId: string } }>()
);
export const MoveUpSuccess = createAction(
	'[Tree Edit] Move Topic One Position Up Success',
	props<{ payload: { namespace: string; topicId: string } }>()
);
export const MoveDownSuccess = createAction(
	'[Tree Edit] Move Topic One Position Down Success',
	props<{ payload: { namespace: string; topicId: string } }>()
);
export const MoveLevelUpSuccess = createAction(
	'[Tree Edit] Move Topic One Level Up Success',
	props<{ payload: { namespace: string; topicId: string } }>()
);
export const MoveLevelDownSuccess = createAction(
	'[Tree Edit] Move Topic One Level Down Success',
	props<{ payload: { namespace: string; topicId: string } }>()
);

export const MoveFailed = createAction('[Tree Edit] Move Topic Failed');
