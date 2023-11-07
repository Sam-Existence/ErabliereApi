import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { DocumentationComponent } from 'src/documentation/documentation.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<DocumentationComponent> = {
  title: 'DocumentationComponent',
  component: DocumentationComponent,
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<DocumentationComponent>;

export const Default: Story = {
  render: (args: DocumentationComponent) => ({
    props: args,
  }),
};
