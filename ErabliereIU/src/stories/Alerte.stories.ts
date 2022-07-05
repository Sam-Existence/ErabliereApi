import { Meta, Story } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { AlerteComponent } from '../alerte/alerte.component';
import faker from '@faker-js/faker';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';
import { AjouterAlerteComponent } from 'src/alerte/ajouter-alerte.component';

export default {
  title: 'AlerteComponent',
  component: AlerteComponent,
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata([
      AjouterAlerteComponent
    ])
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
