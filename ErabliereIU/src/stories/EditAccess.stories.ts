import { Meta } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { EditAccessComponent } from '../access/edit-access.component';
import faker from '@faker-js/faker';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  component: EditAccessComponent,
  decorators: [ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()],
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

export const Display = {
  render: (args: any) => ({
    props: args,
  }),

  args: {
    acces: customerAccess,
  },
};

export const Edit = {
  render: (args: any) => ({
    props: args,
  }),

  args: {
    acces: customerAccess,
    displayEditAccess: true,
  },
};
