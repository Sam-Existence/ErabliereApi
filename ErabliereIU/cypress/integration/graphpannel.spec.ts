import { HomePage } from "../pages/home.page";

describe('Graph pannel test', { testIsolation: false }, () => {
    const homePage = new HomePage();
    const tauxSucreId = "010e708b-a7d0-449e-77e2-08d9d37ca582";
    var baseValue = 23;

    function convertBaseValue(value: number) {
        return (value / 10).toString().replace(',', '.');
    }

    it('Check the base value of "Taux de sucre" pannel', () => {
        homePage.visit()
                .getGraphPannel(tauxSucreId)
                .find('h3')
                .should($t => {
                    const text = $t.text()
                
                    expect(text).to.match(/Taux de sucre/);
                    
                    if (text.indexOf(convertBaseValue(baseValue)) > -1) {
                        baseValue = baseValue + 1;
                    }
                });
    });

    it('Add data in "Taux de sucre" pannel', () => {
        var pannelCompoenent = homePage.getGraphPannel(tauxSucreId);
        var addButton = pannelCompoenent.getAddButton();

        addButton.should('exist');
        addButton.click();

        pannelCompoenent.enterValue(baseValue.toString())
        pannelCompoenent.enterDate();
        
        // There is now a new button 'Ajouter' that appears when the first click happend.
        var addButton = pannelCompoenent.getAddButton();

        addButton.should('exist');
        addButton.click();
    });

    it('Should make the form disappear', () => {
        // Find the button 'Annuler'
        var graphPannel = homePage.getGraphPannel(tauxSucreId);
        var cancelButton = graphPannel.getCancelButton();
        
        cancelButton.should('exist');
        cancelButton.click();

        // The form should not be displayed anymore
        graphPannel.find('input[name="valeur"]').should('not.exist');
        graphPannel.find('input[name="date"]').should('not.exist');
    });

    it('Should see that the data is added', () => {
        var graphPannel = homePage.getGraphPannel(tauxSucreId);

        var title = graphPannel.find('h3');
        title.should('exist');
        title.should('contain', convertBaseValue(baseValue));
    });
});