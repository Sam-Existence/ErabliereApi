import { type Meta, type StoryObj } from '@storybook/angular';
import { AlerteComponent } from 'src/alerte/alerte.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AlerteComponent> = {
  title: 'AlerteComponent',
  component: AlerteComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<AlerteComponent>;

export const Primary: Story = {

};
