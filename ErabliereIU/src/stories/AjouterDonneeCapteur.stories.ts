import { Meta } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { AjouterDonneeCapteurComponent } from '../donneeCapteurs/ajouter-donnee-capteur.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  component: AjouterDonneeCapteurComponent,
  decorators: [ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()],
} as Meta;

var fixture = {};

export const Button = {
  render: (args: any) => ({
    props: args,
  }),

  args: {},
};

export const Form = {
  render: (args: any) => ({
    props: args,
  }),

  args: {
    display: true,
  },
};
