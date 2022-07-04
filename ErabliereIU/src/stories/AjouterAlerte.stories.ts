import { Meta, Story } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { AjouterAlerteComponent } from '../alerte/ajouter-alerte.component';
import faker from '@faker-js/faker';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  title: 'AjouterAlerteComponent',
  component: AjouterAlerteComponent,
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
} as Meta;

var fixture = {};

//👇 We create a “template” of how args map to rendering
const Template: Story = (args) => ({
  props: args
});

//👇 Each story then reuses that template
export const Button = Template.bind({});

Button.args = {
    
};

export const FormAlerteTrioDonnees = Template.bind({});

FormAlerteTrioDonnees.args = {
  display: true,
  typeAlerte: 1
};

export const FormAlerteCapteur = Template.bind({});

FormAlerteCapteur.args = {
  display: true,
  typeAlerte: 2
};