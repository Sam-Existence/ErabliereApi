import type { Meta, StoryObj } from '@storybook/angular';
import { DocumentationComponent } from 'src/documentation/documentation.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<DocumentationComponent> = {
  title: 'DocumentationComponent',
  component: DocumentationComponent,
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
type Story = StoryObj<DocumentationComponent>;

export const Default: Story = {
  render: (args: DocumentationComponent) => ({
    props: args,
  }),
};
