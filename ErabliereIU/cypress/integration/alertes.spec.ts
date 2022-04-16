describe('Alerte functionnality', () => {
    it('Visit the alerte page', () => {
        cy.login()
        cy.visit('/')
        // Find the alertes button in the nav menu
        cy.waitFor('#nav-menu-alerte-button', 5000)

        cy.contains('Alerte').click()

        cy.get('alerte-page.col-lg-10 > h3').should('have.text', 'Alertes')
    });
});