import { HomePage } from '../pages/home.page';
import { faker } from '@faker-js/faker';

describe('Alerte functionnality', { testIsolation: false }, () => {
    const homePage = new HomePage();

    it('Visit the alerte page', () => {
        homePage.visit()
                .clickOnAlerteButtonNavMenu()
                .getPageTitle()
                .should('have.text', 'Alertes')
    });

    it("should add an alerte on 'donnees'", () => {
        let alertePage = homePage.clickOnAlerteButtonNavMenu();

        const email = faker.internet.email().toString();

        alertePage.clickOnAddAlerteButton()
                  .typeEmail(email)
                  .clickOnCreateButton();

        cy.wait(1000);

        alertePage.getLastAlerteDonneesEmail().should('have.text', email);
    })
});