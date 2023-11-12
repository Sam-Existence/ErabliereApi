import { type Meta, type StoryObj } from '@storybook/angular';
import { VacciumGraphPannelComponent } from 'src/donnees/sub-panel/vaccium-graph-pannel.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<VacciumGraphPannelComponent> = {
  title: 'VacciumGraphPannelComponent',
  component: VacciumGraphPannelComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<VacciumGraphPannelComponent>;

export const Default: Story = {
  render: (args: VacciumGraphPannelComponent) => ({
    props: args,
  }),
};
