import { APP_INITIALIZER, NgModule, DoBootstrap, ApplicationRef } from '@angular/core';
import { AppComponent } from './app.component';
import { routes } from './app.routes';
import { HttpClientModule } from '@angular/common/http';
import { EnvironmentService } from 'src/environments/environment.service';
import { MsalService, MSAL_INSTANCE } from '@azure/msal-angular';
import { BrowserCacheLocation, Configuration, IPublicClientApplication, LogLevel, PublicClientApplication } from '@azure/msal-browser';
import { environment } from 'src/environments/environment';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { provideNgxMask } from 'ngx-mask';
import { BrowserModule } from '@angular/platform-browser';
import 'chartjs-adapter-date-fns';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

declare global {
  interface Window {
    appRef: ApplicationRef,
    Cypress: any,
    authorisationFactoryService: AuthorisationFactoryService
   }
}

export function initConfig(appConfig: EnvironmentService) {
  return () => appConfig.loadConfig();
}

export function MSALInstanceFactory(appConfig: EnvironmentService): IPublicClientApplication {
  if (!appConfig.authEnable) {
    return new PublicClientApplication({
      auth: {
        clientId: "null"
      }
    });
  }

  if (appConfig.clientId == undefined) {
    throw new Error("/assets/config/oauth-oidc.json/clientId cannot be null when using AzureAD authentication mode");
  }

  const msalConfig: Configuration = {
    auth: {
      clientId: appConfig.clientId,
      authority: "https://login.microsoftonline.com/" + appConfig.tenantId,
      redirectUri: "/signin-callback",
      postLogoutRedirectUri: "/signout-callback",
      navigateToLoginRequestUrl: true
    },
    cache: {
      cacheLocation: BrowserCacheLocation.LocalStorage,

      // if the web browser used is safari, set storeAuthStateInCookie to true
      // otherwise, set it to false
      storeAuthStateInCookie: window.navigator.userAgent.indexOf("Safari") > -1,
    },
    system: {
      loggerOptions: {
        loggerCallback: (level: LogLevel, message: string, containsPii: boolean): void => {
          if (containsPii) {
            return;
          }
          switch (level) {
            case LogLevel.Error:
              console.error(message);
              return;
            case LogLevel.Info:
              console.info(message);
              return;
            case LogLevel.Verbose:
              console.debug(message);
              return;
            case LogLevel.Warning:
              console.warn(message);
              return;
          }
        },
        piiLoggingEnabled: false
      },
      windowHashTimeout: 60000,
      iframeHashTimeout: 6000,
      loadFrameTimeout: 0,
      asyncPopups: false
    }
  };

  var pca = new PublicClientApplication(msalConfig);

  return pca;
}

@NgModule({
    providers: [
        {
            provide: APP_INITIALIZER,
            useFactory: initConfig,
            deps: [EnvironmentService],
            multi: true,
        },
        {
            provide: MSAL_INSTANCE,
            useFactory: MSALInstanceFactory,
            deps: [EnvironmentService]
        },
        MsalService,
        provideNgxMask(),
        provideRouter(routes, withComponentInputBinding()),
        provideAnimationsAsync()
    ],
    imports: [
        BrowserModule,
        HttpClientModule
    ]
})
export class AppModule implements DoBootstrap {
  constructor() {}

  ngDoBootstrap(appRef: ApplicationRef): void {
    appRef.bootstrap(AppComponent);

    if (!environment.production) {
      if (window.Cypress) {
        window.appRef = appRef;
      }
    }
  }
}
