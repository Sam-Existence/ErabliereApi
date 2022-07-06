import { Meta, Story } from '@storybook/angular';
import { CapteurPannelsComponent } from '../donnees/sub-panel/capteur-pannels.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';
import { GraphiqueComponent } from 'src/graphique/graphique.component';

export default {
  title: 'CapteurPannelsComponent',
  component: CapteurPannelsComponent,
  decorators: [
    ModuleStoryHelper.getErabliereApiStoriesModuleMetadata([
      GraphiqueComponent
    ])
  ]
} as Meta;

//👇 We create a “template” of how args map to rendering
const Template: Story = (args) => ({
  props: args,
});

//👇 Each story then reuses that template
export const Primary = Template.bind({});

Primary.args = {
  
};