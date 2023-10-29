import { Meta } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { DocumentationComponent } from '../documentation/documentation.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  component: DocumentationComponent,
  decorators: [ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()],
} as Meta;

var fixture = {};

export const Primary = {
  render: (args: any) => ({
    props: args,
  }),

  args: {},
};
