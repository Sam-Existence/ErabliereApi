import type { Meta, StoryObj } from '@storybook/angular';
import { AjouterCapteurComponent } from 'src/erablieres/ajouter-capteur.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterCapteurComponent> = {
  title: 'AjouterCapteurComponent',
  component: AjouterCapteurComponent,
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
type Story = StoryObj<AjouterCapteurComponent>;

export const Default: Story = {
  render: (args: AjouterCapteurComponent) => ({
    props: args,
  }),
};
