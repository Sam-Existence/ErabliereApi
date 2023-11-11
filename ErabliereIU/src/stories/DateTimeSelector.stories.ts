import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { DateTimeSelectorComponent } from 'src/donnees/sub-panel/userinput/date-time-selector.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<DateTimeSelectorComponent> = {
  title: 'DateTimeSelectorComponent',
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<DateTimeSelectorComponent>;

export const Default: Story = {
  render: (args: DateTimeSelectorComponent) => ({
    props: args,
  }),
};
