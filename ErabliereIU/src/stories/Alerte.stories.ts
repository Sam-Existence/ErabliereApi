import { Meta } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { AlerteComponent } from '../alerte/alerte.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';
import { AjouterAlerteComponent } from 'src/alerte/ajouter-alerte.component';

export default {
  component: AlerteComponent,
  decorators: [ModuleStoryHelper.getErabliereApiStoriesModuleMetadata([AjouterAlerteComponent])],
} as Meta;

var fixture = {};

export const Primary = {
  render: (args: any) => ({
    props: args,
  }),

  args: {},
};
