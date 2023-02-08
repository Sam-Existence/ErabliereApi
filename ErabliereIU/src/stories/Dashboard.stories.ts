import { Meta } from '@storybook/angular';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  component: DashboardComponent,
  decorators: [ModuleStoryHelper.getErabliereApiStoriesModuleMetadata([])],
} as Meta;

export const Primary = {
  render: (args: any) => ({
    props: args,
  }),

  args: {},
};
