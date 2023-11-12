import type { Meta, StoryObj } from '@storybook/angular';
import { NoteComponent } from 'src/notes/note.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<NoteComponent> = {
  title: 'NoteComponent',
  component: NoteComponent,
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
type Story = StoryObj<NoteComponent>;

export const Default: Story = {
  render: (args: NoteComponent) => ({
    props: args,
  }),
};
