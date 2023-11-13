import { type Meta, type StoryObj } from '@storybook/angular';
import { CapteurPannelsComponent } from 'src/donnees/sub-panel/capteur-pannels.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<CapteurPannelsComponent> = {
  title: 'CapteurPannelsComponent',
  component: CapteurPannelsComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<CapteurPannelsComponent>;

export const Primary: Story = {
  render: (args: CapteurPannelsComponent) => ({
    props: args,
  }),
};
