import { HomePage } from "cypress/pages/home.page";

describe('Buy an api key', function () {
    const homePage = new HomePage();

    it('buy an api key', function () {
        cy.checkoutEnabled().then(enabled => {
            if (enabled) {
                homePage.visit()
                    .clickOnAProposPage()
                    .clickOnBuyApiKey()
                    .then(url => {
                        expect(url).to.contains('https://checkout.stripe.com/pay/');
                    });
            }
            else {
                cy.log('Skipping test because checkout is not enabled');
            }
        })
    })
})