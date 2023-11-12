import type { Meta, StoryObj } from '@storybook/angular';
import { EinputComponent } from 'src/formsComponents/einput.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<EinputComponent> = {
  title: 'EinputComponent',
  component: EinputComponent,
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
type Story = StoryObj<EinputComponent>;

export const Default: Story = {
  render: (args: EinputComponent) => ({
    props: args,
  }),
};
