import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { EditPanelComponent } from './components/edit-panel/edit-panel.component';
import { LayoutRoute } from '../../shared/layout/interfaces';
import { TopicGuard } from '../../shared/topic/guard/topic.guard';

const routes: LayoutRoute = [
	{
		path: ':dcvid',
		component: EditPanelComponent,
		canActivate: [TopicGuard]
	},
	{
		path: 'word',
		loadChildren: () =>
			import('../../containers/word/editword/editword.module').then(
				(m) => m.EditWordContainerModule
			),
		data: {
			fullscreen: true,
			icon: 'word'
		}
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class EditRoutingModule {}
