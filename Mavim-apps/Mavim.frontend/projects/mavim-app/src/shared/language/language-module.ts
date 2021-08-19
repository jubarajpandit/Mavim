import { NgModule, APP_INITIALIZER } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { languageReducer } from './+state';
import { LanguageService } from './service/language.service';
import { LanguageSessionStoreService } from './service/language-session-store.service';
import { LanguageFacade } from './service/language.facade';
import { EffectsModule } from '@ngrx/effects';
import { LanguageEffects } from './+state/language.effects';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { LanguageInterceptor } from './interceptor/language.interceptor';

@NgModule({
	declarations: [],
	imports: [
		CommonModule,
		StoreModule.forFeature('language', languageReducer),
		EffectsModule.forFeature([LanguageEffects])
	],
	exports: [],
	providers: [
		LanguageService,
		{
			provide: APP_INITIALIZER,
			useFactory: (service: LanguageService) => () => {
				service.initializeDatabaseLanguage();
			},
			deps: [LanguageService],
			multi: true
		},
		LanguageSessionStoreService,
		LanguageFacade,
		{
			provide: HTTP_INTERCEPTORS,
			useClass: LanguageInterceptor,
			multi: true
		}
	]
})
export class LanguageModule {}
