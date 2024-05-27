import { type Meta, type StoryObj } from '@storybook/angular';
import { AccessListComponent } from 'src/access/access-list.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AccessListComponent> = {
  title: 'AccessListComponent',
  component: AccessListComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<AccessListComponent>;

export const Primary: Story = {

};
