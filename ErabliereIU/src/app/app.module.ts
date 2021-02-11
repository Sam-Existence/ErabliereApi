import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { ErabliereComponent } from 'src/erablieres/erabliere.component';
import { DonneesComponent } from 'src/donnees/donnees.component';
import { GraphPannelComponent } from 'src/donnees/sub-panel/graphpanel.component';
import { BarilsComponent } from 'src/barils/barils.component';
import { ChartsModule } from 'ng2-charts';

@NgModule({
  declarations: [
    AppComponent,
    ErabliereComponent,
    DonneesComponent,
    GraphPannelComponent,
    BarilsComponent
  ],
  imports: [
    BrowserModule,
    ChartsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
