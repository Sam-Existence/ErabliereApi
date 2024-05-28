import { type Meta, type StoryObj } from '@storybook/angular';
import { AdminCustomerAccessListComponent } from 'src/access/customer-access-list/admin-customer-access-list.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AdminCustomerAccessListComponent> = {
  title: 'CustomerAccessListComponent',
  component: AdminCustomerAccessListComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<AdminCustomerAccessListComponent>;

export const Primary: Story = {
    args: {
        customersAccess: [
            {
                idErabliere: "366912bb-cdbf-44a3-93ec-642e02997685",
                erabliere: {
                    nom: "Saint-Michel",
                },
                idCustomer: "0a215c38-233f-494e-81d4-997c5a3b8abc",
                access: 7
            },
            {
                idErabliere: "366912bb-cdbf-44a3-93ec-622e02997685",
                erabliere: {
                    nom: "Québec",
                },
                idCustomer: "0a215c38-233f-494e-81d4-997c5a3b8abc",
                access: 7
            },
            {
                idErabliere: "366912bb-cdbf-44a3-93ec-642e03997685",
                erabliere: {
                    nom: "Lil Ste-Foy",
                },
                idCustomer: "0a215c38-233f-494e-81d4-997c5a3b8abc",
                access: 7
            },
            {
                idErabliere: "366912bb-cdbf-44a3-93ac-642e02997685",
                erabliere: {
                    nom: "Érablière de Jane",
                },
                idCustomer: "0a215c38-233f-494e-81d4-997c5a3b8abc",
                access: 7
            }
        ]
    }
};
