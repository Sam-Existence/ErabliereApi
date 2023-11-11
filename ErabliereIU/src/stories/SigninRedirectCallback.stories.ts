import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { SigninRedirectCallbackComponent } from 'src/authorisation/signin-redirect/signin-redirect-callback.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<SigninRedirectCallbackComponent> = {
  title: 'SigninRedirectCallbackComponent',
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<SigninRedirectCallbackComponent>;

export const Default: Story = {
  render: (args: SigninRedirectCallbackComponent) => ({
    props: args,
  }),
};
