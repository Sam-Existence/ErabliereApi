import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { SignoutRedirectCallbackComponent } from 'src/authorisation/signout-redirect/signout-redirect-callback.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<SignoutRedirectCallbackComponent> = {
  title: 'SignoutRedirectCallbackComponent',
  component: SignoutRedirectCallbackComponent,
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<SignoutRedirectCallbackComponent>;

export const Default: Story = {
  render: (args: SignoutRedirectCallbackComponent) => ({
    props: args,
  }),
};
