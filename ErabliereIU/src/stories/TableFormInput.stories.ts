import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { TableFormInputComponent } from 'src/formsComponents/table-form-input.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<TableFormInputComponent> = {
  title: 'TableFormInputComponent',
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<TableFormInputComponent>;

export const Default: Story = {
  render: (args: TableFormInputComponent) => ({
    props: args,
  }),
};
