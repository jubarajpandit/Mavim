import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { LayoutRoute } from '../../shared/layout/interfaces';
import { InitComponent } from './components/init.component';

const routes: LayoutRoute = [
	{
		path: '',
		component: InitComponent
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class InitRoutingModule {}
