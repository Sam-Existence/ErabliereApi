import { type Meta, type StoryObj } from '@storybook/angular';
import { SigninRedirectCallbackComponent } from 'src/authorisation/signin-redirect/signin-redirect-callback.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<SigninRedirectCallbackComponent> = {
  title: 'SigninRedirectCallbackComponent',
  component: SigninRedirectCallbackComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<SigninRedirectCallbackComponent>;

export const Primary: Story = {
  render: (args: SigninRedirectCallbackComponent) => ({
    props: args,
  }),
};
