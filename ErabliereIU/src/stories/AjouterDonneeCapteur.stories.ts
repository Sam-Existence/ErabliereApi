import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { AjouterDonneeCapteurComponent } from 'src/donneeCapteurs/ajouter-donnee-capteur.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterDonneeCapteurComponent> = {
  title: 'AjouterDonneeCapteurComponent',
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<AjouterDonneeCapteurComponent>;

export const Default: Story = {
  render: (args: AjouterDonneeCapteurComponent) => ({
    props: args,
  }),
};
