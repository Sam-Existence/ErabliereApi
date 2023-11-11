import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { EntraRedirectComponent } from 'src/app/app-redirect.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<EntraRedirectComponent> = {
  title: 'EntraRedirectComponent',
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<EntraRedirectComponent>;

export const Default: Story = {
  render: (args: EntraRedirectComponent) => ({
    props: args,
  }),
};
