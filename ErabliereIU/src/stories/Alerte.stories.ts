import type { Meta, StoryObj } from '@storybook/angular';
import { AlerteComponent } from 'src/alerte/alerte.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AlerteComponent> = {
  title: 'AlerteComponent',
  component: AlerteComponent,
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
type Story = StoryObj<AlerteComponent>;

export const Default: Story = {
  render: (args: AlerteComponent) => ({
    props: args,
  }),
};
