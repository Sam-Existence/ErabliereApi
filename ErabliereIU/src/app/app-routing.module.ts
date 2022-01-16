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
    { path: '', component: GraphiqueComponent },
    { path: 'dashboard', component: GraphiqueComponent },
    { path: 'alertes', component: AlerteComponent },
    { path: 'documentations', component: DocumentationComponent },
    { path: 'notes', component: NoteComponent },
    { path: 'apropos', component: AProposComponent },
    { path: 'signin-callback', component: SigninRedirectCallbackComponent },
    { path: 'signout-callback', component: SignoutRedirectCallbackComponent }
]

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
})
export class AppRoutingModule { }
