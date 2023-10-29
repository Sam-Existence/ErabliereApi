import { Meta } from '@storybook/angular';
import { Customer } from 'src/model/customer';
import { CustomerAccess } from 'src/model/customerAccess';

import { BarilsComponent } from '../barils/barils.component';
import { ModuleStoryHelper } from './moduleMetadata/moduleStoryHelper';
import { ChartsModule } from 'ng2-charts';
import { BrowserModule } from '@angular/platform-browser';

export default {
  component: BarilsComponent,
  decorators: [ModuleStoryHelper.getErabliereApiStoriesModuleMetadata([], [ChartsModule])],
} as Meta;

var fixture = {};

export const Primary = {
  render: (args: any) => ({
    props: args,
  }),

  args: {},
};
