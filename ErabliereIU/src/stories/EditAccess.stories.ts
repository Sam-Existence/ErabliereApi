import { Meta, Story } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { EditAccessComponent } from '../access/edit-access.component';
import faker from '@faker-js/faker';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  title: 'EditAccessComponent',
  component: EditAccessComponent,
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
} as Meta;

var customerAccess = new CustomerAccess();
customerAccess.id = faker.datatype.uuid();
customerAccess.idCustomer = faker.datatype.uuid();
customerAccess.idErabliere = faker.datatype.uuid();
customerAccess.access = 15; // 15 équivaut au droit d'accès complet
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
