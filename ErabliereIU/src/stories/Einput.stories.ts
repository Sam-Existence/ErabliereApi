import { type Meta, type StoryObj } from '@storybook/angular';
import { EinputComponent } from 'src/formsComponents/einput.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<EinputComponent> = {
  title: 'EinputComponent',
  component: EinputComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<EinputComponent>;

export const Primary: Story = {

};
