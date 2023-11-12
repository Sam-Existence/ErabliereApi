import type { Meta, StoryObj } from '@storybook/angular';
import { AproposComponent } from 'src/apropos/apropos.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AproposComponent> = {
  title: 'AproposComponent',
  component: AproposComponent,
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
type Story = StoryObj<AproposComponent>;

export const Default: Story = {
  render: (args: AproposComponent) => ({
    props: args,
  }),
};
