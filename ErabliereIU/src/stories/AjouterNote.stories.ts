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

export const Default: Story = {
  render: (args: AjouterNoteComponent) => ({
    props: args,
  }),
};
