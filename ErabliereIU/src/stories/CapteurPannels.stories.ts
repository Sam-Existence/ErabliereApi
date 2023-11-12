import type { Meta, StoryObj } from '@storybook/angular';
import { CapteurPannelsComponent } from 'src/donnees/sub-panel/capteur-pannels.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<CapteurPannelsComponent> = {
  title: 'CapteurPannelsComponent',
  component: CapteurPannelsComponent,
  tags: ['autodocs'],
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    // ModuleStoryHelper.getErabliereApiStoriesModuleMetadata(),
    // ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ]
};

export default meta;
type Story = StoryObj<CapteurPannelsComponent>;

export const Default: Story = {
  render: (args: CapteurPannelsComponent) => ({
    props: args,
  }),
};
