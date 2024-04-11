import type { Meta, StoryObj } from '@storybook/angular';
import { AjouterAlerteComponent } from 'src/alerte/ajouter-alerte.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterAlerteComponent> = {
  title: 'AjouterAlerteComponent',
  component: AjouterAlerteComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<AjouterAlerteComponent>;

export const Button: Story = {
  
};

export const FormAlerteCapteur: Story = {
  args: {
    display: true,
    typeAlerte: 1,
  },
};

export const FormAlerteTrioDonnees: Story = {
  args: {
    display: true,
    typeAlerte: 2,
  },
};
