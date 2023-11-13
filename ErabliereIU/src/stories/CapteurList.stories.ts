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

export const Primary: Story = {
  render: (args: CapteurListComponent) => ({
    props: args,
  }),
};
