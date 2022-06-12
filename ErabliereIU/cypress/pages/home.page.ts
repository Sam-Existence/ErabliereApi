import { AlertesPage } from "./alertes.page";
import { NotesPage } from "./notes.page";
import { GraphPannelCompoenent } from "./component/graphpannel.component";
import { AProposPage } from "./apropos.page";

export class HomePage {
    visit(): HomePage {
        cy.login();
        cy.visit('/');
        return this;
    }

    clickOnAlerteButtonNavMenu(): AlertesPage {
        cy.waitFor('#nav-menu-alerte-button', 10000)
        cy.get('#nav-menu-alerte-button', { timeout: 10000 }).click()
        return new AlertesPage();
    }

    clickOnNotesButtonNavMenu(): NotesPage {
        cy.waitFor('#nav-menu-notes-button', 10000)
        cy.get('#nav-menu-notes-button', { timeout: 10000 }).click()
        return new NotesPage();
    }

    clickOnAProposPage(): AProposPage {
        cy.get('.container-fluid > .navbar-collapse > .navbar-nav > .nav-item:nth-child(5) > .nav-link').click();
        return new AProposPage();
    }

    getGraphPannel(guidId: string): GraphPannelCompoenent {
        return new GraphPannelCompoenent('#graph-pannel-' + guidId);
    }
}