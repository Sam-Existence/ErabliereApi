import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { ErabliereComponent } from 'src/erablieres/erabliere.component';
import { DonneesComponent } from 'src/donnees/donnees.component';
import { GraphPannelComponent } from 'src/donnees/sub-panel/graphpanel.component';
import { BarilsComponent } from 'src/barils/barils.component';
import { ChartsModule } from 'ng2-charts';
import { AlerteComponent } from 'src/alerte/alerte.component';
import { CameraComponent } from 'src/camera/camera.component';
import { AProposComponent } from 'src/apropos/apropos.component';
import { DocumentationComponent } from 'src/documentation/documentation.component';
import { BarPannelComponent } from 'src/donnees/sub-panel/barpannel.component';
import { AuthorisationService } from 'src/authorisation/authorisation-service.component';
import { AppRoutingModule } from './app-routing.module';
import { SigninRedirectCallbackComponent } from 'src/authorisation/signin-redirect/signin-redirect-callback.component';
import { DashboardComponent } from 'src/dashboard/dashboard.component';
import { SignoutRedirectCallbackComponent } from 'src/authorisation/signout-redirect/signout-redirect-callback.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CapteurPannelsComponent } from 'src/donnees/sub-panel/capteurpannels.component';

@NgModule({
  declarations: [
    AppComponent,
    ErabliereComponent,
    DonneesComponent,
    DashboardComponent,
    GraphPannelComponent,
    CapteurPannelsComponent,
    BarPannelComponent,
    BarilsComponent,
    AlerteComponent,
    CameraComponent,
    AProposComponent,
    DocumentationComponent,
    SigninRedirectCallbackComponent,
    SignoutRedirectCallbackComponent
  ],
  imports: [
    BrowserModule,
    ChartsModule,
    AppRoutingModule,
    HttpClientModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
