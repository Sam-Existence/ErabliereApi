import { type Meta, type StoryObj } from '@storybook/angular';
import { GraphPannelComponent } from 'src/donnees/sub-panel/graph-pannel.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<GraphPannelComponent> = {
  title: 'GraphPannelComponent',
  component: GraphPannelComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<GraphPannelComponent>;

export const Primary: Story = {
  args: {
    timeaxes: ['2021-01-01 00:00:00', '2021-01-01 00:01:00', '2021-01-01 00:02:00', '2021-01-01 00:03:00', '2021-01-01 00:04:00', '2021-01-01 00:05:00', '2021-01-01 00:06:00', '2021-01-01 00:07:00', '2021-01-01 00:08:00'],
    datasets: [
      { 
          data: [1, 2, 3, 4, 5, 6, 7, 8, 9], 
          label: "Temperature",
          fill: true,
          pointBackgroundColor: 'rgba(255,255,0,0.8)',
          pointBorderColor: 'black',
          tension: 0.5
      }
    ]
  }
};
