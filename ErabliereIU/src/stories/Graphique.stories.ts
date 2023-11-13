import { type Meta, type StoryObj } from '@storybook/angular';
import { GraphiqueComponent } from 'src/graphique/graphique.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<GraphiqueComponent> = {
  title: 'GraphiqueComponent',
  component: GraphiqueComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<GraphiqueComponent>;

export const Primary: Story = {
  render: (args: GraphiqueComponent) => ({
    props: args,
  }),
};
