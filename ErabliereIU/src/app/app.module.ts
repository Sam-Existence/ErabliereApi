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

@NgModule({
  declarations: [
    AppComponent,
    ErabliereComponent,
    DonneesComponent,
    GraphPannelComponent,
    BarPannelComponent,
    BarilsComponent,
    AlerteComponent,
    CameraComponent,
    AProposComponent,
    DocumentationComponent
  ],
  imports: [
    BrowserModule,
    ChartsModule
  ],
  providers: [AuthorisationService],
  bootstrap: [AppComponent]
})
export class AppModule { }
