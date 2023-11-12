import type { Meta, StoryObj } from '@storybook/angular';
import { AjouterDocumentationComponent } from 'src/documentation/ajouter-documentation.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterDocumentationComponent> = {
  title: 'AjouterDocumentationComponent',
  component: AjouterDocumentationComponent,
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
type Story = StoryObj<AjouterDocumentationComponent>;

export const Default: Story = {
  render: (args: AjouterDocumentationComponent) => ({
    props: args,
  }),
};
