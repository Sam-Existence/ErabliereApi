import type { Meta, StoryObj } from '@storybook/angular';
import { NotesComponent } from 'src/notes/notes.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<NotesComponent> = {
  title: 'NotesComponent',
  component: NotesComponent,
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
type Story = StoryObj<NotesComponent>;

export const Default: Story = {
  render: (args: NotesComponent) => ({
    props: args,
  }),
};
