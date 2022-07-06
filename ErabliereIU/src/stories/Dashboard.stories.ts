import { Meta, Story } from '@storybook/angular';
import { DashboardComponent } from '../dashboard/dashboard.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

export default {
  title: 'DashboardComponent',
  component: DashboardComponent,
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata([])
  ]
} as Meta;

//👇 We create a “template” of how args map to rendering
const Template: Story = (args) => ({
  props: args,
});

//👇 Each story then reuses that template
export const Primary = Template.bind({});

Primary.args = {
};
