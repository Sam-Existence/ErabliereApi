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

export const WithData: Story = {
  render: (args: DocumentationComponent) => ({
    props: args,
  }),
  args: {
    documentations: [
      {
        id: 1,
        idErabliere: 1,
        title: 'Test',
        text: 'Test',
        file: '',
        fileExtension: ''
      },
      {
        id: 2,
        idErabliere: 1,
        title: 'Test',
        text: 'Test',
        file: '',
        fileExtension: ''
      }
    ]
  }
};