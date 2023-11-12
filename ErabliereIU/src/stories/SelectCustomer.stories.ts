import { type Meta, type StoryObj } from '@storybook/angular';
import { SelectCustomerComponent } from 'src/customer/select-customer.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<SelectCustomerComponent> = {
  title: 'SelectCustomerComponent',
  component: SelectCustomerComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<SelectCustomerComponent>;

export const Default: Story = {
  render: (args: SelectCustomerComponent) => ({
    props: args,
  }),
};
