import { Meta, Story } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { AjouterDonneeCapteurComponent } from '../donneeCapteurs/ajouter-donnee-capteur.component';
import faker from '@faker-js/faker';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  title: 'AjouterDonneeCapteurComponent',
  component: AjouterDonneeCapteurComponent,
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
export const Button = Template.bind({});

Button.args = {
  
};

export const Form = Template.bind({});

Form.args = {
  display: true
};