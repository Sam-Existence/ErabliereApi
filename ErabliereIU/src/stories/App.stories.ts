import type { Meta, StoryObj } from '@storybook/angular';
import { AppComponent } from 'src/app/app.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AppComponent> = {
  title: 'AppComponent',
  component: AppComponent,
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
type Story = StoryObj<AppComponent>;

export const Default: Story = {
  render: (args: AppComponent) => ({
    props: args,
  }),
};
