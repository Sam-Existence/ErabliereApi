import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { AjouterCapteurComponent } from 'src/erablieres/ajouter-capteur.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterCapteurComponent> = {
  title: 'AjouterCapteurComponent',
  component: AjouterCapteurComponent,
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<AjouterCapteurComponent>;

export const Default: Story = {
  render: (args: AjouterCapteurComponent) => ({
    props: args,
  }),
};
