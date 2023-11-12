import type { Meta, StoryObj } from '@storybook/angular';
import { DonneesComponent } from 'src/donnees/donnees.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<DonneesComponent> = {
  title: 'DonneesComponent',
  component: DonneesComponent,
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
type Story = StoryObj<DonneesComponent>;

export const Default: Story = {
  render: (args: DonneesComponent) => ({
    props: args,
  }),
};
