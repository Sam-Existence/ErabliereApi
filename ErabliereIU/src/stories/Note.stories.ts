import { Meta } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { NoteComponent } from '../notes/note.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  component: NoteComponent,
  decorators: [ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()],
} as Meta;

var fixture = {};

export const Primary = {
  render: (args: any) => ({
    props: args,
  }),

  args: {},
};
