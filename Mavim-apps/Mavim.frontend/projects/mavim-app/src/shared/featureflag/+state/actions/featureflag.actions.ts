import { createAction, props } from '@ngrx/store';

export const featurefagRequest = createAction(
	'[Featureflag] Load featureflags'
);

export const featurefagSuccess = createAction(
	'[Featureflag] Load featureflags Success',
	props<{ payload: string[] }>()
);

export const featurefagFailure = createAction(
	'[Featureflag] Load featureflags Failure'
);
