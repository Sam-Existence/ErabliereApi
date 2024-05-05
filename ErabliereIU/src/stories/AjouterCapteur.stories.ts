import { type Meta, type StoryObj } from '@storybook/angular';
import { AjouterCapteurComponent } from 'src/erablieres/ajouter-capteur.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterCapteurComponent> = {
  title: 'AjouterCapteurComponent',
  component: AjouterCapteurComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<AjouterCapteurComponent>;

export const Primary: Story = {

};
