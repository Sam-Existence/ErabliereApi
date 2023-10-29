import { Meta } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { SelectCustomerComponent } from '../customer/select-customer.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  component: SelectCustomerComponent,
  decorators: [ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()],
} as Meta;

var fixture = {};

export const Primary = {
  render: (args: any) => ({
    props: args,
  }),

  args: {},
};
