import type { Meta, StoryObj } from '@storybook/angular';
import { AjouterNoteComponent } from 'src/notes/ajouter-note.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterNoteComponent> = {
  title: 'AjouterNoteComponent',
  component: AjouterNoteComponent,
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
type Story = StoryObj<AjouterNoteComponent>;

export const Default: Story = {
  render: (args: AjouterNoteComponent) => ({
    props: args,
  }),
};
