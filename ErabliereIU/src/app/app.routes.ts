import { Routes } from '@angular/router';
import { AlerteComponent } from 'src/alerte/alerte.component';
import { AproposComponent } from 'src/apropos/apropos.component';
import { SigninRedirectCallbackComponent } from 'src/authorisation/signin-redirect/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from 'src/authorisation/signout-redirect/signout-redirect-callback.component';
import { DocumentationComponent } from 'src/documentation/documentation.component';
import { ErabliereComponent } from 'src/erablieres/erabliere.component';
import { NotesComponent } from 'src/notes/notes.component';
import {AdminCustomersComponent} from "../admin/admin-customers.component";
import {Page404Component} from "./page404/page404.component";

export const routes: Routes = [
    { path: '', redirectTo: 'e', pathMatch: 'full' },
    { path: 'e', component: ErabliereComponent },
    { path: 'e/:idErabliereSelectionee', redirectTo: 'e/:idErabliereSelectionee/graphiques', pathMatch: 'full' },
    { path: 'e/:idErabliereSelectionee/graphiques', component: ErabliereComponent },
    { path: 'e/:idErabliereSelectionee/alertes', component: AlerteComponent },
    { path: 'e/:idErabliereSelectionee/documentations', component: DocumentationComponent },
    { path: 'e/:idErabliereSelectionee/notes', component: NotesComponent },
    { path: 'apropos', component: AproposComponent },
    { path: 'signin-callback', component: SigninRedirectCallbackComponent },
    { path: 'signout-callback', component: SignoutRedirectCallbackComponent },
    { path: 'admin', redirectTo: 'admin/customers', pathMatch: 'full' },
    { path: 'admin/customers', component: AdminCustomersComponent },
    { path: '**', component: Page404Component }
]
