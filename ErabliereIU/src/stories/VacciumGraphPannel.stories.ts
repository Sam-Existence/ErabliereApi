import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { VacciumGraphPannelComponent } from 'src/donnees/sub-panel/vaccium-graph-pannel.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<VacciumGraphPannelComponent> = {
  title: 'VacciumGraphPannelComponent',
  component: VacciumGraphPannelComponent,
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<VacciumGraphPannelComponent>;

export const Default: Story = {
  render: (args: VacciumGraphPannelComponent) => ({
    props: args,
  }),
};
