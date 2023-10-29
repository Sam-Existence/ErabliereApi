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

        cy.wait(1000);

        alertePage.getLastAlerteDonneesEmail().should('have.text', email);
    });

    // TODO. End this test
    // Edit alerte test in progress
    // it("should edit an alerte on 'donnees'", () => {
    //     let alertePage = homePage.clickOnAlerteButtonNavMenu();

    //     const name = FixtureUtil.getRandomName();

    //     alertePage.clickOnEditAlerteButton()
    //               .typeName(name)
    //               .clickOnEditButton();

    //     cy.wait(1000);

    //     alertePage.getLastAlerteDonneesName().should('have.text', name);
    // });
});