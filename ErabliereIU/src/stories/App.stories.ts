import { type Meta, type StoryObj } from '@storybook/angular';
import { AppComponent } from 'src/app/app.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AppComponent> = {
  title: 'AppComponent',
  component: AppComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<AppComponent>;

export const Primary: Story = {

};
