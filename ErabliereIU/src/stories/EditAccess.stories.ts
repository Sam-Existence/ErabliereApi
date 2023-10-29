import { Meta } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { EditAccessComponent } from '../access/edit-access.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';
import { FixtureUtil } from 'cypress/util/fixtureUtil';

export default {
  component: EditAccessComponent,
  decorators: [ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()],
} as Meta;

var customerAccess = new CustomerAccess();
customerAccess.id = crypto.randomUUID();
customerAccess.idCustomer = crypto.randomUUID();
customerAccess.idErabliere = crypto.randomUUID();
customerAccess.access = 15; // 15 équivaut au droit d'accès complet
customerAccess.customer = new Customer();
customerAccess.customer.id = customerAccess.idCustomer;
customerAccess.customer.name = FixtureUtil.getFirstName();
customerAccess.customer.email = FixtureUtil.getRandomEmail();
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
