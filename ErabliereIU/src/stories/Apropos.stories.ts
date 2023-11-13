import { type Meta, type StoryObj } from '@storybook/angular';
import { AproposComponent } from 'src/apropos/apropos.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AproposComponent> = {
  title: 'AproposComponent',
  component: AproposComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<AproposComponent>;

export const Default: Story = {
  render: (args: AproposComponent) => ({
    props: args,
  }),
};

export const EveryOptions: Story = {
  render: (args: AproposComponent) => ({
    props: args,
  }),
  args: {
    supportEmail: 'example@domain.com',
    checkoutEnabled: true,
  },
};

export const StripeEnabled: Story = {
  render: (args: AproposComponent) => ({
    props: args,
  }),
  args: {
    checkoutEnabled: true,
  },
};

export const WithEmailSupport: Story = {
  render: (args: AproposComponent) => ({
    props: args,
  }),
  args: {
    supportEmail: 'exemple@domain.com',
  },
};
