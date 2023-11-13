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

export const Button: Story = {
  render: (args: AjouterAlerteComponent) => ({
    props: args,
  }),
};

export const FormAlerteCapteur: Story = {
  render: (args: AjouterAlerteComponent) => ({
    props: args,
  }),
  args: {
    display: true,
    typeAlerte: 1,
  },
};

export const FormAlerteTrioDonnees: Story = {
  render: (args: AjouterAlerteComponent) => ({
    props: args,
  }),
  args: {
    display: true,
    typeAlerte: 2,
  },
};
