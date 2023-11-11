import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { GestionCapteursComponent } from 'src/erablieres/gestion-capteurs.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<GestionCapteursComponent> = {
  title: 'GestionCapteursComponent',
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<GestionCapteursComponent>;

export const Default: Story = {
  render: (args: GestionCapteursComponent) => ({
    props: args,
  }),
};
