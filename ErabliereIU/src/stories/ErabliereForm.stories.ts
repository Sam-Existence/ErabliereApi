import { type Meta, type StoryObj } from '@storybook/angular';
import { ErabliereFormComponent } from 'src/erablieres/erabliere-form.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ErabliereFormComponent> = {
  title: 'ErabliereFormComponent',
  component: ErabliereFormComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<ErabliereFormComponent>;

export const Default: Story = {
  render: (args: ErabliereFormComponent) => ({
    props: args,
  }),
};
