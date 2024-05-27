import { type Meta, type StoryObj } from '@storybook/angular';
import { ViewAccessRowComponent } from 'src/access/view-access-row/view-access-row.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ViewAccessRowComponent> = {
  title: 'ViewAccessRowComponent',
  component: ViewAccessRowComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<ViewAccessRowComponent>;

export const Primary: Story = {

};
