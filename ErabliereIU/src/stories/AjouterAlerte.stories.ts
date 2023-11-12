import { type Meta, type StoryObj } from '@storybook/angular';
import { AjouterAlerteComponent } from 'src/alerte/ajouter-alerte.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterAlerteComponent> = {
  title: 'AjouterAlerteComponent',
  component: AjouterAlerteComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<AjouterAlerteComponent>;

export const Default: Story = {
  render: (args: AjouterAlerteComponent) => ({
    props: args,
  }),
};
