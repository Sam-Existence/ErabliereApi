import { type Meta, type StoryObj } from '@storybook/angular';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';
import { AgoraCallServiceComponent } from 'src/dashboard/agora-call-service.component';

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

export default meta;
type Story = StoryObj<AgoraCallServiceComponent>;

export const Primary: Story = {
    render: (args: AgoraCallServiceComponent) => ({
        props: args,
    }),
    args: {

    }
};

export const AfterOneCick: Story = {
    render: (args: AgoraCallServiceComponent) => ({
        props: args,
    }),
    args: {
        showPhone: true
    }
};

export const VideoChat: Story = {
    render: (args: AgoraCallServiceComponent) => ({
        props: args,
    }),
    args: {
        showPhone: true,
        callIsStarted: true
    }
};