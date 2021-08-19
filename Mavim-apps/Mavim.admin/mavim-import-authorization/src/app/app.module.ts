import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { environment } from 'src/environments/environment';

import {
  MsalModule,
  MSAL_CONFIG,
  MSAL_CONFIG_ANGULAR,
  MsalService,
  MsalAngularConfiguration
} from '@azure/msal-angular';
import { Configuration } from 'msal';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MsalInterceptor } from './msal.interceptor';

// https://github.com/AzureAD/microsoft-authentication-library-for-js/tree/dev/lib/msal-angular

export const protectedResourceMap: [string, string[]][] = [
  [environment.apiUrl, ['user.read']]
];

function MSALConfigFactory(): Configuration {
  const { auth, cache } = environment.msalConfig;
  return {
    auth,
    ...cache
  };
}

function MSALAngularConfigFactory(): MsalAngularConfiguration {
  return {
    popUp: false,
    consentScopes: ['user.read'],
    unprotectedResources: ['https://www.microsoft.com/en-us/'],
    protectedResourceMap
  };
}

@NgModule({
  declarations: [AppComponent, HomeComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MsalModule,
    FontAwesomeModule,
    HttpClientModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    {
      provide: MSAL_CONFIG,
      useFactory: MSALConfigFactory
    },
    {
      provide: MSAL_CONFIG_ANGULAR,
      useFactory: MSALAngularConfigFactory
    },
    MsalService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
