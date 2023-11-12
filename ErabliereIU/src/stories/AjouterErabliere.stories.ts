import type { Meta, StoryObj } from '@storybook/angular';
import { AjouterErabliereComponent } from 'src/erablieres/ajouter-erabliere.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterErabliereComponent> = {
  title: 'AjouterErabliereComponent',
  component: AjouterErabliereComponent,
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
type Story = StoryObj<AjouterErabliereComponent>;

export const Default: Story = {
  render: (args: AjouterErabliereComponent) => ({
    props: args,
  }),
};
