import type { Meta, StoryObj } from '@storybook/angular';
import { AjouterAlerteComponent } from 'src/alerte/ajouter-alerte.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterAlerteComponent> = {
  title: 'AjouterAlerteComponent',
  component: AjouterAlerteComponent,
  tags: ['autodocs'],
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata(),
    // ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ]
};

export default meta;
type Story = StoryObj<AjouterAlerteComponent>;

export const Default: Story = {
  render: (args: AjouterAlerteComponent) => ({
    props: args,
  }),
};
