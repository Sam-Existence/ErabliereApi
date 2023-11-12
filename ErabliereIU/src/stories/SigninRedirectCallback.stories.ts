import type { Meta, StoryObj } from '@storybook/angular';
import { SigninRedirectCallbackComponent } from 'src/authorisation/signin-redirect/signin-redirect-callback.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<SigninRedirectCallbackComponent> = {
  title: 'SigninRedirectCallbackComponent',
  component: SigninRedirectCallbackComponent,
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
type Story = StoryObj<SigninRedirectCallbackComponent>;

export const Default: Story = {
  render: (args: SigninRedirectCallbackComponent) => ({
    props: args,
  }),
};
