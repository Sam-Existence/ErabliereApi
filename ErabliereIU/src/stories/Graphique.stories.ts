import { type Meta, type StoryObj } from '@storybook/angular';
import { GraphiqueComponent } from 'src/graphique/graphique.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<GraphiqueComponent> = {
  title: 'GraphiqueComponent',
  component: GraphiqueComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<GraphiqueComponent>;

export const Primary: Story = {
  render: (args: GraphiqueComponent) => ({
    props: args,
  }),
  args: {
    erabliere: {
      afficherTrioDonnees: true,
      afficherSectionDompeux: true,
      afficherSectionBaril: true,
      nom: 'Érablière du Lac-Beauport',
    }
  }
};

export const WeatherForecast = {
  render: (args: GraphiqueComponent) => ({
    props: args,
  }),
  args: {
    erabliere: {
      codePostal: 'G3B 2S6'
    }
  }
};

export const AllGraph = {
  render: (args: GraphiqueComponent) => ({
    props: args,
  }),
  args: {
    erabliere: {
      codePostal: 'G3B 2S6',
      afficherTrioDonnees: true,
      afficherSectionDompeux: true,
      capteurs: [
        {
          id: "some-guid",
          nom: "Capteur 1",
          type:"vitesse vent",
          symbole: "km/h",
        }
      ],
    }
  }
};