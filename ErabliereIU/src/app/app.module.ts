import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { ErabliereComponent } from 'src/erablieres/erabliere.component';
import { DonneesComponent } from 'src/donnees/donnees.component';
import { GraphPannelComponent } from 'src/donnees/sub-panel/graphpanel.component';
import { BarilsComponent } from 'src/barils/barils.component';
import { ChartsModule } from 'ng2-charts';
import { AlerteComponent } from 'src/alerte/alerte.component';
import { AProposComponent } from 'src/apropos/apropos.component';
import { DocumentationComponent } from 'src/documentation/documentation.component';
import { BarPannelComponent } from 'src/donnees/sub-panel/barpannel.component';
import { AppRoutingModule } from './app-routing.module';
import { SigninRedirectCallbackComponent } from 'src/authorisation/signin-redirect/signin-redirect-callback.component';
import { DashboardComponent } from 'src/dashboard/dashboard.component';
import { SignoutRedirectCallbackComponent } from 'src/authorisation/signout-redirect/signout-redirect-callback.component';
import { HttpClientModule } from '@angular/common/http';
import { CapteurPannelsComponent } from 'src/donnees/sub-panel/capteurpannels.component';
import { EnvironmentService } from 'src/environments/environment.service';
import { GraphiqueComponent } from 'src/graphique/graphique.component';
import { AjouterAlerteComponent } from 'src/alerte/ajouter-alerte.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ModifierAlerteComponent } from 'src/alerte/modifier-alerte.component';
import { NoteComponent } from 'src/note/note.component';
import { AjouterNoteComponent } from 'src/note/ajouter-note.component';

export function initConfig(appConfig: EnvironmentService) {
  return () => appConfig.loadConfig();
}

const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

@NgModule({
  declarations: [
    AppComponent,
    ErabliereComponent,
    GraphiqueComponent,
    DonneesComponent,
    DashboardComponent,
    GraphPannelComponent,
    CapteurPannelsComponent,
    BarPannelComponent,
    BarilsComponent,
    AlerteComponent,
    AjouterAlerteComponent,
    ModifierAlerteComponent,
    AProposComponent,
    DocumentationComponent,
    NoteComponent,
    AjouterNoteComponent,
    SigninRedirectCallbackComponent,
    SignoutRedirectCallbackComponent
  ],
  imports: [
    BrowserModule,
    ChartsModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [{
    provide: APP_INITIALIZER,
    useFactory: initConfig,
    deps: [EnvironmentService],
    multi: true,
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
