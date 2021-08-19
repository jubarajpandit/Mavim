import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { environment } from '../environments/environment';
import { EffectsModule } from '@ngrx/effects';

// router loaders
import { NotificationsModule } from '../shared/notification/notification.module';
import { ComponentsModule } from '../shared/components/components.module';
import { SecurityModule } from '../shared/security/security.module';
import { RoutingModule } from '../shared/router/router.module';

import { reducers } from './+state';
import { TopicModule } from '../shared/topic/topic.module';
import { AuthorizationModule } from '../shared/authorization/authorization.module';
import { LayoutModule } from '../shared/layout/layout.module';
import { DatabaseModule } from '../shared/database/database.module';
import { LanguageModule } from '../shared/language/language-module';
import { FeatureflagModule } from '../shared/featureflag/featureflag.module';
import { ModalFactoryModule } from '../shared/modal/components/modalfactory/modalfactory.module';
import { TopicMetaModule } from '../shared/topic-meta/topic-meta.module';

@NgModule({
	declarations: [AppComponent],
	imports: [
		BrowserModule,
		SecurityModule,
		RoutingModule.forRoot(),
		NotificationsModule,
		AppRoutingModule,
		ComponentsModule,
		HttpClientModule,
		TopicModule,
		TopicMetaModule,
		LanguageModule,
		LayoutModule,
		ModalFactoryModule,
		StoreModule.forRoot(reducers, {
			runtimeChecks: {
				strictStateImmutability: true,
				strictActionImmutability: true,
				strictStateSerializability: true,
				// tree actions seem not to be serializable (reason unkown),
				// in order to avoid the error we set this to false now:
				strictActionSerializability: false,
				strictActionWithinNgZone: true,
				strictActionTypeUniqueness: true
			}
		}),
		StoreDevtoolsModule.instrument({
			name: 'Mavim Improve',
			maxAge: 25, // Retains last 25 states
			logOnly: environment.production // Restrict extension to log-only mode
		}),
		EffectsModule.forRoot([]),
		AuthorizationModule,
		FeatureflagModule,
		DatabaseModule
	],
	providers: [],
	bootstrap: [AppComponent]
})
export class AppModule {}
