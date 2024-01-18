import { APP_INITIALIZER, NgModule, DoBootstrap, ApplicationRef } from '@angular/core';
import { AppComponent } from './app.component';
import { ErabliereComponent } from 'src/erablieres/erabliere.component';
import { DonneesComponent } from 'src/donnees/donnees.component';
import { GraphPannelComponent } from 'src/donnees/sub-panel/graph-pannel.component';
import { BarilsComponent } from 'src/barils/barils.component';
import { AlerteComponent } from 'src/alerte/alerte.component';
import { AproposComponent } from 'src/apropos/apropos.component';
import { DocumentationComponent } from 'src/documentation/documentation.component';
import { BarPannelComponent } from 'src/donnees/sub-panel/bar-pannel.component';
import { routes } from './app.routes';
import { SigninRedirectCallbackComponent } from 'src/authorisation/signin-redirect/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from 'src/authorisation/signout-redirect/signout-redirect-callback.component';
import { HttpClientModule } from '@angular/common/http';
import { CapteurPannelsComponent } from 'src/donnees/sub-panel/capteur-pannels.component';
import { EnvironmentService } from 'src/environments/environment.service';
import { AjouterAlerteComponent } from 'src/alerte/ajouter-alerte.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModifierAlerteComponent } from 'src/alerte/modifier-alerte.component';
import { NotesComponent } from 'src/notes/notes.component';
import { AjouterNoteComponent } from 'src/notes/ajouter-note.component';
import { AjouterDonneeCapteurComponent } from 'src/donneeCapteurs/ajouter-donnee-capteur.component';
import { MsalService, MSAL_INSTANCE, MsalRedirectComponent } from '@azure/msal-angular';
import { BrowserCacheLocation, Configuration, IPublicClientApplication, LogLevel, PublicClientApplication } from '@azure/msal-browser';
import { environment } from 'src/environments/environment';
import { AuthorisationFactoryService } from 'src/authorisation/authorisation-factory-service';
import { DateTimeSelectorComponent } from 'src/donnees/sub-panel/userinput/date-time-selector.component';
import { NoteComponent } from 'src/notes/note.component';
import { AjouterErabliereComponent } from 'src/erablieres/ajouter-erabliere.component';
import { ErabliereFormComponent } from 'src/erablieres/erabliere-form.component';
import { ModifierErabliereComponent } from 'src/erablieres/modifier-erabliere.component';
import { ModifierAccesUtilisateursComponent } from 'src/erablieres/modifier-acces-utilisateurs.component';
import { SelectCustomerComponent } from 'src/customer/select-customer.component';
import { EditAccessComponent } from 'src/access/edit-access.component';
import { InputErrorComponent } from "../formsComponents/input-error.component";
import { VacciumGraphPannelComponent } from 'src/donnees/sub-panel/vaccium-graph-pannel.component';
import { GestionCapteursComponent } from 'src/erablieres/gestion-capteurs.component';
import { AjouterCapteurComponent } from 'src/erablieres/ajouter-capteur.component';
import { CapteurListComponent } from 'src/erablieres/capteur-list.component';
import { TableFormInputComponent } from 'src/formsComponents/table-form-input.component';
import { AjouterDocumentationComponent } from 'src/documentation/ajouter-documentation.component';
import { EinputComponent } from 'src/formsComponents/einput.component';
import { NgxMaskDirective, NgxMaskPipe, provideEnvironmentNgxMask, provideNgxMask } from 'ngx-mask';
import { NgChartsModule } from 'ng2-charts';
import { BrowserModule } from '@angular/platform-browser';
import { EntraRedirectComponent } from './entra-redirect.component';
import 'chartjs-adapter-date-fns';
import { SiteNavBarComponent } from 'src/dashboard/site-nav-bar.component';
import { YouAreNotConnectedComponent } from 'src/dashboard/you-are-note-connected.component';
import { provideRouter, withComponentInputBinding } from '@angular/router';

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
      storeAuthStateInCookie: false,
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

  return new PublicClientApplication(msalConfig);
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
        provideRouter(routes, withComponentInputBinding())
    ],
    imports: [
        BrowserModule,
        NgChartsModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule,
        NgxMaskDirective,
        NgxMaskPipe,
        AppComponent,
        ErabliereComponent,
        AjouterErabliereComponent,
        ModifierErabliereComponent,
        ErabliereFormComponent,
        ModifierAccesUtilisateursComponent,
        SelectCustomerComponent,
        EditAccessComponent,
        DonneesComponent,
        GraphPannelComponent,
        VacciumGraphPannelComponent,
        DateTimeSelectorComponent,
        CapteurPannelsComponent,
        GestionCapteursComponent,
        AjouterCapteurComponent,
        CapteurListComponent,
        BarPannelComponent,
        BarilsComponent,
        AlerteComponent,
        AjouterAlerteComponent,
        ModifierAlerteComponent,
        AproposComponent,
        DocumentationComponent,
        AjouterDocumentationComponent,
        NotesComponent,
        NoteComponent,
        AjouterNoteComponent,
        AjouterDonneeCapteurComponent,
        EntraRedirectComponent,
        SigninRedirectCallbackComponent,
        SignoutRedirectCallbackComponent,
        EinputComponent,
        InputErrorComponent,
        TableFormInputComponent,
        SiteNavBarComponent,
        YouAreNotConnectedComponent
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
