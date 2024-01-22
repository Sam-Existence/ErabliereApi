import { type Meta, type StoryObj } from '@storybook/angular';
import { CapteurListComponent } from 'src/erablieres/capteur-list.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<CapteurListComponent> = {
  title: 'CapteurListComponent',
  component: CapteurListComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<CapteurListComponent>;

export const Default: Story = {
  render: (args: CapteurListComponent) => ({
    props: args,
  }),
  args: {
    capteurs: [
      {
        id: "1",
        nom: 'Capteur 1',
        symbole: '%',
        ajouterDonneeDepuisInterface: true,
        afficherCapteurDashboard: true,
        dc: new Date(2024, 1, 1, 0, 0, 0, 0).toString(),
      },
      {
        id: "2",
        nom: 'Capteur 2',
        symbole: '°C',
        ajouterDonneeDepuisInterface: true,
        afficherCapteurDashboard: false,
        dc: new Date(2024, 1, 1, 0, 0, 0, 0).toString(),
      },
      {
        id: "3",
        nom: 'Capteur 3',
        symbole: '°F',
        ajouterDonneeDepuisInterface: false,
        afficherCapteurDashboard: false,
        dc: new Date(2024, 1, 1, 0, 0, 0, 0).toString(),
      }
    ]
  }
};

export const Empty: Story = {
  render: (args: CapteurListComponent) => ({
    props: args,
  }),
};
