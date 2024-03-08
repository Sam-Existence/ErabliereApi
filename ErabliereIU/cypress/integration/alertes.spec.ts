import { FixtureUtil } from 'cypress/util/fixtureUtil';
import { HomePage } from '../pages/home.page';

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

        const email = FixtureUtil.getRandomEmail();

        alertePage.clickOnAddAlerteButton()
                  .typeEmail(email)
                  .clickOnCreateButton();

        cy.wait(2000);

        alertePage.getLastAlerteDonneesEmail().should('have.text', email);
    });
});