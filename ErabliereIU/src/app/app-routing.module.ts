import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AlerteComponent } from 'src/alerte/alerte.component';
import { AProposComponent } from 'src/apropos/apropos.component';
import { SigninRedirectCallbackComponent } from 'src/authorisation/signin-redirect/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from 'src/authorisation/signout-redirect/signout-redirect-callback.component';
import { DashboardComponent } from 'src/dashboard/dashboard.component';
import { DocumentationComponent } from 'src/documentation/documentation.component';
import { GraphiqueComponent } from 'src/graphique/graphique.component';
import { NoteComponent } from 'src/note/note.component';

const routes: Routes = [
    { path: '', component: DashboardComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'alertes', component: DashboardComponent },
    { path: 'documentations', component: DashboardComponent },
    { path: 'notes', component: DashboardComponent },
    { path: 'apropos', component: DashboardComponent },
    { path: 'signin-callback', component: SigninRedirectCallbackComponent },
    { path: 'signout-callback', component: SignoutRedirectCallbackComponent }
]

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule { }
