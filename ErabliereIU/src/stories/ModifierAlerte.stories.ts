import { type Meta, type StoryObj } from '@storybook/angular';
import { ModifierAlerteComponent } from 'src/alerte/modifier-alerte.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ModifierAlerteComponent> = {
  title: 'ModifierAlerteComponent',
  component: ModifierAlerteComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<ModifierAlerteComponent>;

export const Primary: Story = {
  render: (args: ModifierAlerteComponent) => ({
    props: args,
  }),
};
