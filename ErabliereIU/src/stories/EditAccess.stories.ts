import { Meta, moduleMetadata, Story } from '@storybook/angular';
import { HttpClientModule } from '@angular/common/http';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { EditAccessCompoenent } from '../access/edit-access.component';
import { MsalService, MSAL_INSTANCE } from '@azure/msal-angular';
import { PublicClientApplication } from '@azure/msal-browser';
import faker from '@faker-js/faker';

export default {
  title: 'EditAccessComponent',
  component: EditAccessCompoenent,
  decorators: [
    moduleMetadata({
      imports: [HttpClientModule],
      providers: [
        { provide: MSAL_INSTANCE, useValue: new PublicClientApplication({
          auth: {
            clientId: "null"
          }
        })},
        MsalService
      ]
    })
  ]
} as Meta;

var customerAccess = new CustomerAccess();
customerAccess.id = faker.datatype.uuid();
customerAccess.idCustomer = faker.datatype.uuid();
customerAccess.idErabliere = faker.datatype.uuid();
customerAccess.access = faker.datatype.number({min: 0, max: 15});
customerAccess.customer = new Customer();
customerAccess.customer.id = customerAccess.idCustomer;
customerAccess.customer.name = faker.name.firstName();
customerAccess.customer.email = faker.internet.email();
customerAccess.customer.uniqueName = customerAccess.customer.email;

//👇 We create a “template” of how args map to rendering
const Template: Story = (args) => ({
  props: args,
});

//👇 Each story then reuses that template
export const Display = Template.bind({});

Display.args = {
  acces: customerAccess
};

export const Edit = Template.bind({});

Edit.args = {
  acces: customerAccess,
  displayEditAccess: true
};
