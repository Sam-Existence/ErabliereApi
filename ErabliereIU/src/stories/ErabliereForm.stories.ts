import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { ErabliereFormComponent } from 'src/erablieres/erabliere-form.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ErabliereFormComponent> = {
  title: 'ErabliereFormComponent',
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<ErabliereFormComponent>;

export const Default: Story = {
  render: (args: ErabliereFormComponent) => ({
    props: args,
  }),
};
