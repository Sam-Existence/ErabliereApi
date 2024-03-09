import { type Meta, type StoryObj } from '@storybook/angular';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';
import { AgoraCallServiceComponent } from 'src/dashboard/agora-call-service.component';
import { Customer } from 'src/model/customer';

const meta: Meta<AgoraCallServiceComponent> = {
    title: 'AgoraCallServiceComponent',
    component: AgoraCallServiceComponent,
    tags: ['autodocs'],
    parameters: {
        layout: 'fullscreen',
    },
    decorators: [
        ModuleStoryHelper.getErabliereApiStoriesApplicationConfig()
    ],
};

const customers: Customer[] = [
    {
        id: 1,
        name: 'John',
        email: 'john@doe.com'
    },
    {
        id: 2,
        name: 'Jane',
        email: 'jane@doe.com'
    }
];

export default meta;
type Story = StoryObj<AgoraCallServiceComponent>;

export const Primary: Story = {
    render: (args: AgoraCallServiceComponent) => ({
        props: args,
    }),
    args: {
        userList: customers
    }
};

export const AfterOneCick: Story = {
    render: (args: AgoraCallServiceComponent) => ({
        props: args,
    }),
    args: {
        userList: customers,
        showPhone: true,
    }
};

export const VideoChat: Story = {
    render: (args: AgoraCallServiceComponent) => ({
        props: args,
    }),
    args: {
        userList: customers,
        showPhone: true,
        callIsStarted: true,
        userUid: 123,
        selectedUser: 'Jane'
    }
};