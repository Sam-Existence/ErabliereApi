import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { ModifierAlerteComponent } from 'src/alerte/modifier-alerte.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ModifierAlerteComponent> = {
  title: 'ModifierAlerteComponent',
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<ModifierAlerteComponent>;

export const Default: Story = {
  render: (args: ModifierAlerteComponent) => ({
    props: args,
  }),
};
