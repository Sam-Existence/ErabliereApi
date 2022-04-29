export class AlertesPage {
    getPageTitle(): Cypress.Chainable<JQuery<HTMLElement>> {
        return cy.get('alerte-page.col-lg-10 > h3')
    }
}