import type { Meta, StoryObj } from '@storybook/angular';
import { moduleMetadata } from '@storybook/angular';
import { CommonModule } from '@angular/common';

import { AjouterNoteComponent } from 'src/notes/ajouter-note.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';

const meta: Meta<AjouterNoteComponent> = {
  title: 'AjouterNoteComponent',
  component: AjouterNoteComponent,
  parameters: {
    // More on how to position stories at: https://storybook.js.org/docs/angular/configure/story-layout
    layout: 'fullscreen',
  },
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata()
  ]
};

export default meta;
type Story = StoryObj<AjouterNoteComponent>;

export const Default: Story = {
  render: (args: AjouterNoteComponent) => ({
    props: args,
  }),
};
