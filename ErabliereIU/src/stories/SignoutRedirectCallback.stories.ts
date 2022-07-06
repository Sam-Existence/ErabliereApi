import { Meta, Story } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { SignoutRedirectCallbackComponent } from '../authorisation/signout-redirect/signout-redirect-callback.component';
import faker from '@faker-js/faker';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  title: 'SignoutRedirectCallbackComponent',
  component: SignoutRedirectCallbackComponent,
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
export const Primary = Template.bind({});

Primary.args = {
  
};
