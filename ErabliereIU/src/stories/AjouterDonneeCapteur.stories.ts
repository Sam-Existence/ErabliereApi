import type { Meta, StoryObj } from '@storybook/angular';
import { AjouterDonneeCapteurComponent } from 'src/donneeCapteurs/ajouter-donnee-capteur.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterDonneeCapteurComponent> = {
  title: 'AjouterDonneeCapteurComponent',
  component: AjouterDonneeCapteurComponent,
  tags: ['autodocs'],
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    // ModuleStoryHelper.getErabliereApiStoriesModuleMetadata(),
    // ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ]
};

export default meta;
type Story = StoryObj<AjouterDonneeCapteurComponent>;

export const Default: Story = {
  render: (args: AjouterDonneeCapteurComponent) => ({
    props: args,
  }),
};
