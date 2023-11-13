import { type Meta, type StoryObj } from '@storybook/angular';
import { DocumentationComponent } from 'src/documentation/documentation.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<DocumentationComponent> = {
  title: 'DocumentationComponent',
  component: DocumentationComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<DocumentationComponent>;

export const Primary: Story = {
  render: (args: DocumentationComponent) => ({
    props: args,
  }),
};
