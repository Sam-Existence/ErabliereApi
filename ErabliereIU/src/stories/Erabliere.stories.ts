import { type Meta, type StoryObj } from '@storybook/angular';
import { ErabliereComponent } from 'src/erablieres/erabliere.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ErabliereComponent> = {
  title: 'ErabliereComponent',
  component: ErabliereComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<ErabliereComponent>;

export const Primary: Story = {
  render: (args: ErabliereComponent) => ({
    props: args,
  }),
};
