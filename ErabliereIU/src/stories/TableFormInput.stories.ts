import { type Meta, type StoryObj } from '@storybook/angular';
import { TableFormInputComponent } from 'src/formsComponents/table-form-input.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<TableFormInputComponent> = {
  title: 'TableFormInputComponent',
  component: TableFormInputComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<TableFormInputComponent>;

export const Primary: Story = {

};
