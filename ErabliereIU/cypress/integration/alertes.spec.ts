import { HomePage } from '../pages/home.page';
import { AlertesPage } from '../pages/alertes.page';

describe('Alerte functionnality', () => {
    const homePage = new HomePage();

    it('Visit the alerte page', () => {
        homePage.visit()
                .clickOnAlerteButtonNavMenu()
                .getPageTitle()
                .should('have.text', 'Alertes')
    });
});