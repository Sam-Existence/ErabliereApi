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
    args: {
        erablieres: customers
    }
};

export const AfterOneCick: Story = {
    args: {
        erablieres: customers,
        showPhone: true,
    }
};

export const VideoChat: Story = {
    args: {
        erablieres: customers,
        showPhone: true,
        callIsStarted: true,
        userUid: 123,
        erabliereId: 'Jane'
    }
};