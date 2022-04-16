describe('Graph pannel test', () => {
    const tauxSucreId = "010e708b-a7d0-449e-77e2-08d9d37ca582";
    var baseValue = 23;

    function convertBaseValue(value: number) {
        return (value / 10).toString().replace(',', '.');
    }

    it('Check the base value of "Taux de sucre" pannel', () => {
        cy.login()
        cy.visit('/')
        cy.get('#graph-pannel-' + tauxSucreId).then($pannel => {
            var title = cy.wrap($pannel).find('h3')
            title.should(($t) => {
                const text = $t.text()
              
                expect(text).to.match(/Taux de sucre/);
                
                if (text.indexOf(convertBaseValue(baseValue)) > -1) {
                    baseValue = baseValue + 1;
                }
              })
        });
    });

    it('Add data in "Taux de sucre" pannel', () => {
        cy.get('#graph-pannel-' + tauxSucreId).then($pannel => {
            var buttons = cy.wrap($pannel).find('button');
            var addButton = buttons.filter(function(index, element) {
                return element.innerText === "Ajouter";
            });
            addButton.should('exist');
            addButton.click();

            var valeurField = cy.wrap($pannel).find('input[name="valeur"]');
            valeurField.type(baseValue.toString());

            var dateField = cy.wrap($pannel).find('input[name="date"]');
            var localDate = new Date().toLocaleString('ca-FR');
            var dateAndTime = localDate.split(',');
            var dateInfo = dateAndTime[0].split('/');
            var timeInfo = dateAndTime[1].split(':');
            dateField.type(dateInfo[2].padStart(2, '0') + '-' + dateInfo[1].padStart(2, '0') + '-' + dateInfo[0].padStart(2, '0') + 'T' + timeInfo[0].trim().padStart(2, '0') + ':' + timeInfo[1].padStart(2, '0'));

            // There is now a new button 'Ajouter' that appears when the first click happend.
            var buttons = cy.wrap($pannel).find('button');
            var addButton = buttons.filter(function(index, element) {
                return element.innerText === "Ajouter";
            });

            addButton.should('exist');
            addButton.click();
        });
    });

    it('Should make the form disappear', () => {
        // Find the button 'Annuler'
        cy.get('#graph-pannel-' + tauxSucreId).then($pannel => {
            var buttons = cy.wrap($pannel).find('button');
            var cancelButton = buttons.filter(function(index, element) {
                return element.innerText === "Annuler";
            });
            cancelButton.should('exist');
            cancelButton.click();

            // The form should not be displayed anymore
            cy.wrap($pannel).find('input[name="valeur"]').should('not.exist');
            cy.wrap($pannel).find('input[name="date"]').should('not.exist');
        });
    });

    it('Should see that the data is added', () => {
        cy.get('#graph-pannel-' + tauxSucreId).then($pannel => {
            var title = cy.wrap($pannel).find('h3');
            title.should('exist');
            title.should('contain', convertBaseValue(baseValue));
        });
    });
});