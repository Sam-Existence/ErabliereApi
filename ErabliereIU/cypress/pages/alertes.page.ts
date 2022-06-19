import { FormUtil } from "cypress/util/formUtil"

export class AlertesPage {
    getPageTitle(): Cypress.Chainable<JQuery<HTMLElement>> {
        return cy.get('alerte-page.col-lg-10 > h3')
    }
    clickOnAddAlerteButton(): AlertesPage {
        FormUtil.clickButton("alerte-page", "ajouter-alerte-btn");
        return this;
    }
    typeEmail(email: string): AlertesPage {
        FormUtil.typeText(email, "alerte-page", "destinataire");
        return this;
    }
    clickOnCreateButton(): AlertesPage {
        FormUtil.clickButton("alerte-page", "creer-alerte-donnees-btn");
        return this;
    }
    getLastAlerteDonneesEmail(): Cypress.Chainable<JQuery<HTMLElement>> {
        return cy.get('alerte-page').then($obj => {
            // find the last row of the table and validate the email
            return $obj.find("table tbody tr td:nth-child(3)").last();
        });
    }
}