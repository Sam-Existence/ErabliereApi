import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { ModifierErabliereComponent } from 'src/erablieres/modifier-erabliere.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ModifierErabliereComponent> = {
  title: 'ModifierErabliereComponent',
  component: ModifierErabliereComponent,
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<ModifierErabliereComponent>;

export const Default: Story = {
  render: (args: ModifierErabliereComponent) => ({
    props: args,
  }),
};
