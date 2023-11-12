import type { Meta, StoryObj } from '@storybook/angular';
import { TableFormInputComponent } from 'src/formsComponents/table-form-input.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<TableFormInputComponent> = {
  title: 'TableFormInputComponent',
  component: TableFormInputComponent,
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
type Story = StoryObj<TableFormInputComponent>;

export const Default: Story = {
  render: (args: TableFormInputComponent) => ({
    props: args,
  }),
};
