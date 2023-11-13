import { type Meta, type StoryObj } from '@storybook/angular';
import { DateTimeSelectorComponent } from 'src/donnees/sub-panel/userinput/date-time-selector.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<DateTimeSelectorComponent> = {
  title: 'DateTimeSelectorComponent',
  component: DateTimeSelectorComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<DateTimeSelectorComponent>;

export const Primary: Story = {
  render: (args: DateTimeSelectorComponent) => ({
    props: args,
  }),
};
