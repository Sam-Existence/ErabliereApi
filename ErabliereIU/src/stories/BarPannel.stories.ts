import { type Meta, type StoryObj } from '@storybook/angular';
import { BarPannelComponent } from 'src/donnees/sub-panel/bar-pannel.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<BarPannelComponent> = {
  title: 'BarPannelComponent',
  component: BarPannelComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<BarPannelComponent>;

export const Primary: Story = {
  render: (args: BarPannelComponent) => ({
    props: args,
  }),
};
