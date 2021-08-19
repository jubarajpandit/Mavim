import { NgModule, APP_INITIALIZER } from '@angular/core';
import { StoreModule, Store } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { AuthorizationEffects } from './+state/effects/authorization.effects';
import { authorizationReducer } from './+state/reducers/authorization.reducers';
import { AuthorizationService } from './service/authorization.service';
import * as fromAuthorization from './+state';
import { AuthorizationFacade } from './service/authorization.facade';

export function initAuthorization(
	authorizationStore: Store<fromAuthorization.State>
): () => void {
	return (): void => {
		new AuthorizationFacade(authorizationStore).loadAuthorization();
	};
}

@NgModule({
	declarations: [],
	imports: [
		StoreModule.forFeature('authorization', authorizationReducer),
		EffectsModule.forFeature([AuthorizationEffects])
	],
	providers: [
		AuthorizationService,
		{
			provide: APP_INITIALIZER,
			useFactory: initAuthorization,
			deps: [Store],
			multi: true
		}
	]
})
export class AuthorizationModule {}
