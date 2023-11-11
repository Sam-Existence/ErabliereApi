import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { GraphPannelComponent } from 'src/donnees/sub-panel/graph-pannel.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<GraphPannelComponent> = {
  title: 'GraphPannelComponent',
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<GraphPannelComponent>;

export const Default: Story = {
  render: (args: GraphPannelComponent) => ({
    props: args,
  }),
};
