import type { Meta, StoryObj } from '@storybook/angular';
import { BarilsComponent } from 'src/barils/barils.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<BarilsComponent> = {
  title: 'BarilsComponent',
  component: BarilsComponent,
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
type Story = StoryObj<BarilsComponent>;

export const Default: Story = {
  render: (args: BarilsComponent) => ({
    props: args,
  }),
};
