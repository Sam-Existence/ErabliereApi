import { type Meta, type StoryObj } from '@storybook/angular';
import { AproposComponent } from 'src/apropos/apropos.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AproposComponent> = {
  title: 'AproposComponent',
  component: AproposComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<AproposComponent>;

export const Default: Story = {
  render: (args: AproposComponent) => ({
    props: args,
  }),
};
