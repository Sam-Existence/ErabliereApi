import { type Meta, type StoryObj } from '@storybook/angular';
import { GestionCapteursComponent } from 'src/erablieres/gestion-capteurs.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<GestionCapteursComponent> = {
  title: 'GestionCapteursComponent',
  component: GestionCapteursComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<GestionCapteursComponent>;

export const Primary: Story = {

};
