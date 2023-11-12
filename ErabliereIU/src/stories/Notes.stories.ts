import { type Meta, type StoryObj } from '@storybook/angular';
import { NotesComponent } from 'src/notes/notes.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<NotesComponent> = {
  title: 'NotesComponent',
  component: NotesComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<NotesComponent>;

export const Default: Story = {
  render: (args: NotesComponent) => ({
    props: args,
  }),
};
