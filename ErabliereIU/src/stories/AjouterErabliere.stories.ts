import { type Meta, type StoryObj } from '@storybook/angular';
import { AjouterErabliereComponent } from 'src/erablieres/ajouter-erabliere.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterErabliereComponent> = {
  title: 'AjouterErabliereComponent',
  component: AjouterErabliereComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<AjouterErabliereComponent>;

export const Primary: Story = {
  render: (args: AjouterErabliereComponent) => ({
    props: args,
  }),
};
