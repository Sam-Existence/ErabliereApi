import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { InputErrorComponent } from 'src/formsComponents/input-error.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<InputErrorComponent> = {
  title: 'InputErrorComponent',
  component: InputErrorComponent,
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<InputErrorComponent>;

export const Default: Story = {
  render: (args: InputErrorComponent) => ({
    props: args,
  }),
};
