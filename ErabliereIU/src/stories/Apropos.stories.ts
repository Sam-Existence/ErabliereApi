import { Meta, Story } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { AproposComponent } from '../apropos/apropos.component';
import faker from '@faker-js/faker';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  title: 'AproposComponent',
  component: AproposComponent,
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
} as Meta;

var fixture = {};

//👇 We create a “template” of how args map to rendering
const Template: Story = (args) => ({
  props: args,
});

//👇 Each story then reuses that template
export const Default = Template.bind({});

Default.args = {
  
};

export const WithEmaiSupport = Template.bind({});

WithEmaiSupport.args = {
  supportEmail: 'exemple@domain.com'
};

export const StripeEnabled = Template.bind({});

StripeEnabled.args = {
  checkoutEnabled: true
};

export const EveryOptions = Template.bind({});

EveryOptions.args = {
  supportEmail: 'example@domain.com',
  checkoutEnabled: true
};