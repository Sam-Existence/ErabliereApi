import type { Meta, StoryObj } from '@storybook/angular';
import { BarPannelComponent } from 'src/donnees/sub-panel/bar-pannel.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<BarPannelComponent> = {
  title: 'BarPannelComponent',
  component: BarPannelComponent,
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
type Story = StoryObj<BarPannelComponent>;

export const Default: Story = {
  render: (args: BarPannelComponent) => ({
    props: args,
  }),
};
