import { type Meta, type StoryObj } from '@storybook/angular';
import { AjouterNoteComponent } from 'src/notes/ajouter-note.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterNoteComponent> = {
  title: 'AjouterNoteComponent',
  component: AjouterNoteComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<AjouterNoteComponent>;

export const Button: Story = {

};

export const Form: Story = {
  args: {
    display: true
  }
};