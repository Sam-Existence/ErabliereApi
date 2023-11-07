import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { ModifierAccesUtilisateursComponent } from 'src/erablieres/modifier-acces-utilisateurs.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ModifierAccesUtilisateursComponent> = {
  title: 'ModifierAccesUtilisateursComponent',
  component: ModifierAccesUtilisateursComponent,
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<ModifierAccesUtilisateursComponent>;

export const Default: Story = {
  render: (args: ModifierAccesUtilisateursComponent) => ({
    props: args,
  }),
};
