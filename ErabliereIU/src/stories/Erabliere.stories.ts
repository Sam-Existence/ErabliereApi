import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { ErabliereComponent } from 'src/erablieres/erabliere.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ErabliereComponent> = {
  title: 'ErabliereComponent',
  component: ErabliereComponent,
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<ErabliereComponent>;

export const Default: Story = {
  render: (args: ErabliereComponent) => ({
    props: args,
  }),
};
