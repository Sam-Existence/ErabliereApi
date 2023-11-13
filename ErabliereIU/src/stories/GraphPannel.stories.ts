import { type Meta, type StoryObj } from '@storybook/angular';
import { GraphPannelComponent } from 'src/donnees/sub-panel/graph-pannel.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<GraphPannelComponent> = {
  title: 'GraphPannelComponent',
  component: GraphPannelComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<GraphPannelComponent>;

export const Primary: Story = {
  render: (args: GraphPannelComponent) => ({
    props: args,
  }),
};
