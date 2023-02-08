import { Meta } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { ErabliereFormComponent } from '../erablieres/erabliere-form.component';
import faker from '@faker-js/faker';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  component: ErabliereFormComponent,
  decorators: [ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()],
} as Meta;

var fixture = {};

export const Primary = {
  render: (args: any) => ({
    props: args,
  }),

  args: {},
};
