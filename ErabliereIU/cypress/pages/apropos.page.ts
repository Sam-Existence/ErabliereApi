export class AProposPage {
    clickOnBuyApiKey(): void {
        cy.get('div > .container > .col-12 > div > .btn').click()

        cy.wait(5000);

        cy.url().then(stripeCheckoutUrl => {
            cy.log(stripeCheckoutUrl);
            
            cy.forceVisit(stripeCheckoutUrl);
        });
    }
}