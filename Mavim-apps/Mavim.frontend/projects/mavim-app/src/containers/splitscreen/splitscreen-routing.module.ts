import { NgModule, Type } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SplitScreenComponent } from './components/splitscreen/splitscreen.component';
import { TopicComponent } from '../topic/components/topic/topic.component';
import { LayoutRoute } from '../../shared/layout/interfaces';

export const splitScreenRoute = (
	path: string,
	component: Type<unknown>
): LayoutRoute => {
	return [
		{ path, component, outlet: 'left' },
		{ path, component, outlet: 'right' }
	];
};

const routes: LayoutRoute = [
	{
		path: 'nav',
		component: SplitScreenComponent,
		children: [...splitScreenRoute('topic/:dcvid', TopicComponent)]
	}
];
@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class SplitScreenRoutingModule {}
