import { type Meta, type StoryObj } from '@storybook/angular';
import { ErabliereAccessListComponent } from 'src/access/erabliere-access-list/erabliere-access-list.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ErabliereAccessListComponent> = {
  title: 'AccessListComponent',
  component: ErabliereAccessListComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<ErabliereAccessListComponent>;

export const Primary: Story = {

};
