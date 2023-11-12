import type { Meta, StoryObj } from '@storybook/angular';
import { CapteurListComponent } from 'src/erablieres/capteur-list.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<CapteurListComponent> = {
  title: 'CapteurListComponent',
  component: CapteurListComponent,
  tags: ['autodocs'],
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    // ModuleStoryHelper.getErabliereApiStoriesModuleMetadata(),
    // ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ]
};

export default meta;
type Story = StoryObj<CapteurListComponent>;

export const Default: Story = {
  render: (args: CapteurListComponent) => ({
    props: args,
  }),
};
