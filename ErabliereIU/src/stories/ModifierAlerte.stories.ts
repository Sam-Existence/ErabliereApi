import type { Meta, StoryObj } from '@storybook/angular';
import { ModifierAlerteComponent } from 'src/alerte/modifier-alerte.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ModifierAlerteComponent> = {
  title: 'ModifierAlerteComponent',
  component: ModifierAlerteComponent,
  tags: ['autodocs'],
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    // ModuleStoryHelper.getErabliereApiStoriesModuleMetadata(),
    // ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ]
};

export default meta;
type Story = StoryObj<ModifierAlerteComponent>;

export const Default: Story = {
  render: (args: ModifierAlerteComponent) => ({
    props: args,
  }),
};
