import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { environment } from '../../environments/environment';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { MsalModule, MsalAngularConfiguration } from '@azure/msal-angular';
import { Configuration } from 'msal';
import { MsalInterceptor } from './msal.interceptor';
import { scopes } from './constants';

const protectedResourceMap: [string, string[]][] = [
	[`${environment.baseApiUrl}/**`, scopes],
	[`${environment.wopiSrc}/**`, scopes]
];

export function MSALConfigFactory(): Configuration {
	return {
		auth: {
			clientId: environment.clientId,
			authority: 'https://login.microsoftonline.com/common/',
			validateAuthority: true,
			redirectUri: environment.baseUrl,
			postLogoutRedirectUri: environment.baseUrl,
			navigateToLoginRequestUrl: true
		},
		cache: {
			cacheLocation: 'localStorage',
			storeAuthStateInCookie: false // set to true for IE 11
		}
	};
}

export function MSALAngularConfigFactory(): MsalAngularConfiguration {
	return {
		popUp: false,
		consentScopes: scopes,
		unprotectedResources: ['https://www.microsoft.com/en-us/'],
		protectedResourceMap,
		extraQueryParameters: {}
	};
}

@NgModule({
	declarations: [],
	imports: [
		CommonModule,
		MsalModule.forRoot(MSALConfigFactory(), MSALAngularConfigFactory())
	],
	providers: [
		{
			provide: HTTP_INTERCEPTORS,
			useClass: MsalInterceptor,
			multi: true
		}
	]
})
export class SecurityModule {}
