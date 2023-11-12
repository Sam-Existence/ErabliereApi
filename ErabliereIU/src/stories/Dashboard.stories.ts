import type { Meta, StoryObj } from '@storybook/angular';
import { DashboardComponent } from 'src/dashboard/dashboard.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<DashboardComponent> = {
  title: 'DashboardComponent',
  component: DashboardComponent,
  tags: ['autodocs'],
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    // ModuleStoryHelper.getErabliereApiStoriesModuleMetadata(),
    // ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ]
};

export default meta;
type Story = StoryObj<DashboardComponent>;

export const Default: Story = {
  render: (args: DashboardComponent) => ({
    props: args,
  }),
};
