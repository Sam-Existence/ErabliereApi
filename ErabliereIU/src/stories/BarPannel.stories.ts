import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { BarPannelComponent } from 'src/donnees/sub-panel/bar-pannel.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<BarPannelComponent> = {
  title: 'BarPannelComponent',
  component: BarPannelComponent,
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<BarPannelComponent>;

export const Default: Story = {
  render: (args: BarPannelComponent) => ({
    props: args,
  }),
};
