import { NgModule, APP_INITIALIZER } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule, Store } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { databaseReducer } from './+state/reducers/database.reducers';
import { DatabaseEffects } from './+state/effects/database.effects';
import { DatabaseService } from './service/database.service';
import { DatabaseFacade } from './service/database.facade';
import { DatabaseBrowserService } from './service/database-local-store.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { DatabaseInterceptor } from './interceptor/database.interceptor';

export function initDatabase(store: Store): () => void {
	return (): void => {
		new DatabaseFacade(store).initDatabase();
	};
}

export function loadDatabase(store: Store): () => void {
	return (): void => {
		new DatabaseFacade(store).loadDatabase();
	};
}

@NgModule({
	declarations: [],
	imports: [
		CommonModule,
		StoreModule.forFeature('database', databaseReducer),
		EffectsModule.forFeature([DatabaseEffects])
	],
	providers: [
		DatabaseService,
		DatabaseBrowserService,
		{
			provide: APP_INITIALIZER,
			useFactory: initDatabase,
			deps: [Store],
			multi: true
		},
		{
			provide: APP_INITIALIZER,
			useFactory: loadDatabase,
			deps: [Store],
			multi: true
		},
		{
			provide: HTTP_INTERCEPTORS,
			useClass: DatabaseInterceptor,
			multi: true
		}
	]
})
export class DatabaseModule {}
