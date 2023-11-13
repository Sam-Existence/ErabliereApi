import { type Meta, type StoryObj } from '@storybook/angular';
import { NoteComponent } from 'src/notes/note.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<NoteComponent> = {
  title: 'NoteComponent',
  component: NoteComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<NoteComponent>;

export const Primary: Story = {
  render: (args: NoteComponent) => ({
    props: args,
  }),
};
