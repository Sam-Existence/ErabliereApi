import type { Meta, StoryObj } from '@storybook/angular';
import { GestionCapteursComponent } from 'src/erablieres/gestion-capteurs.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<GestionCapteursComponent> = {
  title: 'GestionCapteursComponent',
  component: GestionCapteursComponent,
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
type Story = StoryObj<GestionCapteursComponent>;

export const Default: Story = {
  render: (args: GestionCapteursComponent) => ({
    props: args,
  }),
};
