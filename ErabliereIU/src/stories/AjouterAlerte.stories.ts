import { Meta } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { AjouterAlerteComponent } from '../alerte/ajouter-alerte.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  component: AjouterAlerteComponent,
  decorators: [ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()],
} as Meta;

var fixture = {};

export const Button = {
  render: (args: any) => ({
    props: args,
  }),

  args: {},
};

export const FormAlerteTrioDonnees = {
  render: (args: any) => ({
    props: args,
  }),

  args: {
    display: true,
    typeAlerte: 1,
  },
};

export const FormAlerteCapteur = {
  render: (args: any) => ({
    props: args,
  }),

  args: {
    display: true,
    typeAlerte: 2,
  },
};
