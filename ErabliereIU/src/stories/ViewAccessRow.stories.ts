import { type Meta, type StoryObj } from '@storybook/angular';
import { ViewErabliereAccessRowComponent } from 'src/access/erabliere-access-list/view-erabliere-access-row/view-erabliere-access-row.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ViewErabliereAccessRowComponent> = {
  title: 'ViewAccessRowComponent',
  component: ViewErabliereAccessRowComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<ViewErabliereAccessRowComponent>;

export const Primary: Story = {

};
