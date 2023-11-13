import { type Meta, type StoryObj } from '@storybook/angular';
import { BarilsComponent } from 'src/barils/barils.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<BarilsComponent> = {
  title: 'BarilsComponent',
  component: BarilsComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<BarilsComponent>;

export const Primary: Story = {
  render: (args: BarilsComponent) => ({
    props: args,
  }),
};
