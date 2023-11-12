import { type Meta, type StoryObj } from '@storybook/angular';
import { SignoutRedirectCallbackComponent } from 'src/authorisation/signout-redirect/signout-redirect-callback.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<SignoutRedirectCallbackComponent> = {
  title: 'SignoutRedirectCallbackComponent',
  component: SignoutRedirectCallbackComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<SignoutRedirectCallbackComponent>;

export const Default: Story = {
  render: (args: SignoutRedirectCallbackComponent) => ({
    props: args,
  }),
};
