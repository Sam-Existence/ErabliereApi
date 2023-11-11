import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { SelectCustomerComponent } from 'src/customer/select-customer.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<SelectCustomerComponent> = {
  title: 'SelectCustomerComponent',
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<SelectCustomerComponent>;

export const Default: Story = {
  render: (args: SelectCustomerComponent) => ({
    props: args,
  }),
};
