import type { Meta, StoryObj } from '@storybook/angular';
import { GraphiqueComponent } from 'src/graphique/graphique.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<GraphiqueComponent> = {
  title: 'GraphiqueComponent',
  component: GraphiqueComponent,
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
type Story = StoryObj<GraphiqueComponent>;

export const Default: Story = {
  render: (args: GraphiqueComponent) => ({
    props: args,
  }),
};
