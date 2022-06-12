import { HomePage } from "cypress/pages/home.page";

describe('Buy an api key', function () {
    const homePage = new HomePage();

    it('buy an api key', function () {
        cy.checkoutEnabled().then(enabled => {
            if (enabled) {

                var aproposPage = homePage.visit()
                    .clickOnAProposPage();

                aproposPage.clickOnBuyApiKey();

                cy.get('.FormFieldGroup-child > .FormFieldInput > .CheckoutInputContainer > .InputContainer > #email', { timeout: 20000 }).click()

                cy.get('.FormFieldGroup-child > .FormFieldInput > .CheckoutInputContainer > .InputContainer > #email').type('erabliereapi@freddycoder.com')

                cy.get('.FormFieldGroup-child > .FormFieldInput > .CheckoutInputContainer > .InputContainer > #cardNumber').type('4242 4242 4242 4242')

                cy.get('.FormFieldGroup-child > .FormFieldInput > .CheckoutInputContainer > .InputContainer > #cardExpiry').type('12 / 36')

                cy.get('.FormFieldGroup-child > .FormFieldInput > .CheckoutInputContainer > .InputContainer > #cardCvc').type('123')

                cy.get('.FormFieldGroup-child > .FormFieldInput > .CheckoutInputContainer > .InputContainer > #billingName').type('John Doe')

                cy.get('.FormFieldGroup-child > .FormFieldInput > .CheckoutInputContainer > .InputContainer > #billingPostalCode').type('A1B 2C3')

                cy.get('form > .PaymentForm-confirmPaymentContainer > .flex-item > .SubmitButton > .SubmitButton-IconContainer').click()

                homePage.visit();
            }
            else {
                cy.log('Skipping test because checkout is not enabled');
            }
        })
    })
})