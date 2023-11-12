import type { Meta, StoryObj } from '@storybook/angular';
import { DateTimeSelectorComponent } from 'src/donnees/sub-panel/userinput/date-time-selector.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<DateTimeSelectorComponent> = {
  title: 'DateTimeSelectorComponent',
  component: DateTimeSelectorComponent,
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
type Story = StoryObj<DateTimeSelectorComponent>;

export const Default: Story = {
  render: (args: DateTimeSelectorComponent) => ({
    props: args,
  }),
};
