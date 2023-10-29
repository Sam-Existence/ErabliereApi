import { Meta } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { AproposComponent } from '../apropos/apropos.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  component: AproposComponent,
  decorators: [ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()],
} as Meta;

var fixture = {};

export const Default = {
  render: (args: any) => ({
    props: args,
  }),

  args: {},
};

export const WithEmaiSupport = {
  render: (args: any) => ({
    props: args,
  }),

  args: {
    supportEmail: 'exemple@domain.com',
  },
};

export const StripeEnabled = {
  render: (args: any) => ({
    props: args,
  }),

  args: {
    checkoutEnabled: true,
  },
};

export const EveryOptions = {
  render: (args: any) => ({
    props: args,
  }),

  args: {
    supportEmail: 'example@domain.com',
    checkoutEnabled: true,
  },
};
