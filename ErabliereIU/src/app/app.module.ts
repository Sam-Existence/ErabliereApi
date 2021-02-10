import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { ErabliereComponent } from 'src/erablieres/erabliere.component';
import { DonneesComponent } from 'src/donnees/donnees.component';
import { DompeuxComponent } from 'src/dompeux/dompeux.component';
import { BarilsComponent } from 'src/barils/barils.component';
import { ChartsModule } from 'ng2-charts';
import { TemperatureComponent } from 'src/donnees/sub-panel/temperature.component';
import { VacciumComponent } from 'src/donnees/sub-panel/vaccium.component';
import { NiveauBassinCompoenent } from 'src/donnees/sub-panel/niveaubassin.component';

@NgModule({
  declarations: [
    AppComponent,
    ErabliereComponent,
    DonneesComponent,
    DompeuxComponent,
    BarilsComponent,
    TemperatureComponent,
    VacciumComponent,
    NiveauBassinCompoenent
  ],
  imports: [
    BrowserModule,
    ChartsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
