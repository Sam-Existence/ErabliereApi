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
import {ClientViewComponent} from "./client-view/client-view.component";
import {AdminViewComponent} from "./admin-view/admin-view.component";

export const routes: Routes = [
    {
        path: 'admin',
        component: AdminViewComponent,
        children: [
            {
                path: '',
                redirectTo: 'customers',
                pathMatch: 'full'
            },
            {
                path: 'customers',
                title: 'Érablière Admin - Clients',
                component: AdminCustomersComponent,
            },
            {
                path: '**',
                title: 'Érablière Admin - 404 page non trouvée',
                component: Page404Component
            }
        ]

    },
    {
        path: '',
        component: ClientViewComponent,
        children: [
            {
                path: '',
                redirectTo: 'e',
                pathMatch: 'full'
            },
            {
                path: 'e',
                title: 'ÉrablièreIU - Dashboard',
                component: ErabliereComponent
            },
            {
                path: 'e/:idErabliereSelectionee',
                redirectTo: 'e/:idErabliereSelectionee/graphiques',
                pathMatch: 'full'
            },
            {
                path: 'e/:idErabliereSelectionee/graphiques',
                title: 'ÉrablièreIU - Dashboard',
                component: ErabliereComponent
            },
            {
                path: 'e/:idErabliereSelectionee/alertes',
                title: 'ÉrablièreIU - Alertes',
                component: AlerteComponent
            },
            {
                path: 'e/:idErabliereSelectionee/documentations',
                title: 'ÉrablièreIU - Documentation',
                component: DocumentationComponent
            },
            {
                path: 'e/:idErabliereSelectionee/notes',
                title: 'ÉrablièreIU - Notes',
                component: NotesComponent
            },
            {
                path: 'apropos',
                title: 'ÉrablièreIU - À propos',
                component: AproposComponent
            },
            {
                path: '**',
                title: 'ÉrablièreIU - 404 page non trouvée',
                component: Page404Component
            }
        ]
    },
    { path: 'signin-callback', component: SigninRedirectCallbackComponent },
    { path: 'signout-callback', component: SignoutRedirectCallbackComponent }
]
