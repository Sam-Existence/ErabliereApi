import type { Meta, StoryObj } from '@storybook/angular';
import { VacciumGraphPannelComponent } from 'src/donnees/sub-panel/vaccium-graph-pannel.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<VacciumGraphPannelComponent> = {
  title: 'VacciumGraphPannelComponent',
  component: VacciumGraphPannelComponent,
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
type Story = StoryObj<VacciumGraphPannelComponent>;

export const Default: Story = {
  render: (args: VacciumGraphPannelComponent) => ({
    props: args,
  }),
};
