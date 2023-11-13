import { type Meta, type StoryObj } from '@storybook/angular';
import { AjouterDonneeCapteurComponent } from 'src/donneeCapteurs/ajouter-donnee-capteur.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterDonneeCapteurComponent> = {
  title: 'AjouterDonneeCapteurComponent',
  component: AjouterDonneeCapteurComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<AjouterDonneeCapteurComponent>;

export const Button: Story = {
  render: (args: AjouterDonneeCapteurComponent) => ({
    props: args,
  }),
};

export const Form: Story = {
  render: (args: AjouterDonneeCapteurComponent) => ({
    props: args,
  }),
  args: {
    display: true
  }
};
