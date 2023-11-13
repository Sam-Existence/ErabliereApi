import { type Meta, type StoryObj } from '@storybook/angular';
import { ModifierErabliereComponent } from 'src/erablieres/modifier-erabliere.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ModifierErabliereComponent> = {
  title: 'ModifierErabliereComponent',
  component: ModifierErabliereComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<ModifierErabliereComponent>;

export const Primary: Story = {
  render: (args: ModifierErabliereComponent) => ({
    props: args,
  }),
};
