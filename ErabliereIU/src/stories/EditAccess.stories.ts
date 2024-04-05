import { type Meta, type StoryObj } from '@storybook/angular';
import { EditAccessComponent } from 'src/access/edit-access.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<EditAccessComponent> = {
  title: 'EditAccessComponent',
  component: EditAccessComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<EditAccessComponent>;

export const Primary: Story = {

};
