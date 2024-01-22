import { type Meta, type StoryObj } from '@storybook/angular';
import { AjouterDocumentationComponent } from 'src/documentation/ajouter-documentation.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterDocumentationComponent> = {
  title: 'AjouterDocumentationComponent',
  component: AjouterDocumentationComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<AjouterDocumentationComponent>;

export const Display: Story = {
  render: (args: AjouterDocumentationComponent) => ({
    props: args,
  }),
  args: {
    display: true,
  }
};

export const Hidden: Story = {
  render: (args: AjouterDocumentationComponent) => ({
    props: args,
  }),
};

