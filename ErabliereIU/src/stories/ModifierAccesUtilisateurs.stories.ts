import { type Meta, type StoryObj } from '@storybook/angular';
import { ModifierAccesUtilisateursComponent } from 'src/erablieres/modifier-acces-utilisateurs.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ModifierAccesUtilisateursComponent> = {
  title: 'ModifierAccesUtilisateursComponent',
  component: ModifierAccesUtilisateursComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<ModifierAccesUtilisateursComponent>;

export const Primary: Story = {

};
