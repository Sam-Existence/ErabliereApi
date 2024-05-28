import { type Meta, type StoryObj } from '@storybook/angular';
import { ErabliereAccessListComponent } from 'src/access/erabliere-access-list/erabliere-access-list.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<ErabliereAccessListComponent> = {
  title: 'ErabliereAccessListComponent',
  component: ErabliereAccessListComponent,
  tags: ['autodocs'],
  parameters: {
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
  ],
};

export default meta;
type Story = StoryObj<ErabliereAccessListComponent>;

export const Primary: Story = {
    args: {
        customersAccess: [
            {
                idErabliere: "366912bb-cdbf-44a3-93ec-642e02997685",
                idCustomer: "0a215c38-233f-494e-81d4-997c5acb8abc",
                customer: {
                    name: "James Smith",
                    email: "james@smith.com"
                },
                access: 15
            },
            {
                idErabliere: "366912bb-cdbf-44a3-93ec-642e02997685",
                idCustomer: "0a215c38-233f-494e-81d4-927c5acb8abc",
                customer: {
                    name: "John Smith",
                    email: "john@smith.com"
                },
                access: 8
            },
            {
                idErabliere: "366912bb-cdbf-44a3-93ec-642e02997685",
                idCustomer: "0a215c38-233f-494e-81d4-997c5a3b8abc",
                customer: {
                    name: "Jane Smith",
                    email: "jane@smith.com"
                },
                access: 7
            },
            {
                idErabliere: "366912bb-cdbf-44a3-93ec-642e02997685",
                idCustomer: "0a215c38-233f-494e-81da-997c5acb8abc",
                customer: {
                    name: "Joel Smith",
                    email: "joel@smith.com"
                },
                access: 3
            }
        ]
    }
};
