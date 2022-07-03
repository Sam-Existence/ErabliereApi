// Button.stories.ts

import { Meta, moduleMetadata, Story } from '@storybook/angular';
import { HttpClientModule } from '@angular/common/http';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { EditAccessCompoenent } from '../access/edit-access.component';
import { MsalService, MSAL_INSTANCE } from '@azure/msal-angular';
import { PublicClientApplication } from '@azure/msal-browser';

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

var customer = new CustomerAccess();
customer.id = 1;
customer.idCustomer = 2;
customer.idErabliere = 3;
customer.access = 15;
customer.customer = new Customer();
customer.customer.id = 2;
customer.customer.name = 'Test';
customer.customer.email = 'Test@test.com';
customer.customer.uniqueName = 'Test@test.com';

//👇 We create a “template” of how args map to rendering
const Template: Story = (args) => ({
  props: args,
});

//👇 Each story then reuses that template
export const Display = Template.bind({});

Display.args = {
  acces: customer
};

export const Edit = Template.bind({});

Edit.args = {
  acces: customer,
  displayEditAccess: true
};
  