import { type Meta, type StoryObj } from '@storybook/angular';
import { InputErrorComponent } from 'src/formsComponents/input-error.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<InputErrorComponent> = {
  title: 'InputErrorComponent',
  component: InputErrorComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<InputErrorComponent>;

export const Primary: Story = {

};
