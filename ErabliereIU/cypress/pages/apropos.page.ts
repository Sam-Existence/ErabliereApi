export class AProposPage {

    /**
     * Click sur le bouton pour acheter une clé d'api
     * et retourn l'url de la page
     * @returns Cypress.Chainable<string> - l'url de la page
     * @example https://checkout.stripe.com/pay/cs_test_a1...
     */
    clickOnBuyApiKey(): Cypress.Chainable<string> {
        cy.get('div > .container > .col-12 > div > .btn').click()

        cy.wait(5000);

        return cy.url();
    }
}