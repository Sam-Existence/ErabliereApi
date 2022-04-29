import { AlertesPage } from "./alertes.page";
import { GraphPannelCompoenent } from "./component/graphpannel.component";

export class HomePage {
    visit(): HomePage {
        cy.login();
        cy.visit('/');
        return this;
    }

    clickOnAlerteButtonNavMenu(): AlertesPage {
        cy.waitFor('#nav-menu-alerte-button', 10000)
        cy.contains('Alerte').click()
        return new AlertesPage();
    }

    getGraphPannel(guidId: string): GraphPannelCompoenent {
        return new GraphPannelCompoenent('#graph-pannel-' + guidId);
    }
}