import { type Meta, type StoryObj } from '@storybook/angular';
import { ErabliereComponent } from 'src/erablieres/erabliere.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';
import { ErabliereAIComponent } from 'src/erabliereai/erabliereai-chat.component';

const meta: Meta<ErabliereAIComponent> = {
  title: 'ErabliereAIComponent',
  component: ErabliereAIComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<ErabliereAIComponent>;

export const Primary: Story = {
  args: {
    chatOpen: true
  }
};
