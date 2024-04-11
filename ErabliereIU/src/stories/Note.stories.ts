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
  args: {
    note: {
      id: 1,
      idErabliere: 1,
      title: 'Test',
      text: 'Test',
      noteDate: '2021-01-01',
      file: '',
      fileExtension: '',
      decodedTextFile: ''
    }
  }
};

