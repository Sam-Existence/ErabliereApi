import { Meta } from '@storybook/angular';
import { CapteurPannelsComponent } from '../donnees/sub-panel/capteur-pannels.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';
import { GraphiqueComponent } from 'src/graphique/graphique.component';

export default {
  component: CapteurPannelsComponent,
  decorators: [ModuleStoryHelper.getErabliereApiStoriesModuleMetadata([GraphiqueComponent])],
} as Meta;

export const Primary = {
  render: (args: any) => ({
    props: args,
  }),

  args: {},
};
