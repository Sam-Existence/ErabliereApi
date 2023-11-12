import { type Meta, type StoryObj } from '@storybook/angular';
import { DonneesComponent } from 'src/donnees/donnees.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<DonneesComponent> = {
  title: 'DonneesComponent',
  component: DonneesComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<DonneesComponent>;

export const Default: Story = {
  render: (args: DonneesComponent) => ({
    props: args,
  }),
};
