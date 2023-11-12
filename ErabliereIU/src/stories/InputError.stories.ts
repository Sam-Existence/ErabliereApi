import type { Meta, StoryObj } from '@storybook/angular';
import { InputErrorComponent } from 'src/formsComponents/input-error.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<InputErrorComponent> = {
  title: 'InputErrorComponent',
  component: InputErrorComponent,
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
type Story = StoryObj<InputErrorComponent>;

export const Default: Story = {
  render: (args: InputErrorComponent) => ({
    props: args,
  }),
};
