import { type Meta, type StoryObj } from '@storybook/angular';
import { DashboardComponent } from 'src/dashboard/dashboard.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<DashboardComponent> = {
  title: 'DashboardComponent',
  component: DashboardComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<DashboardComponent>;

export const Primary: Story = {
  render: (args: DashboardComponent) => ({
    props: args,
  }),
};
