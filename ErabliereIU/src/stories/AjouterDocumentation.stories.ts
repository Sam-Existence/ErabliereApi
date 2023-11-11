import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { AjouterDocumentationComponent } from 'src/documentation/ajouter-documentation.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterDocumentationComponent> = {
  title: 'AjouterDocumentationComponent',
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<AjouterDocumentationComponent>;

export const Default: Story = {
  render: (args: AjouterDocumentationComponent) => ({
    props: args,
  }),
};
