import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NewWordComponent } from './components/new-word.component';
import { LayoutRoute } from '../../../shared/layout/interfaces';

const routes: LayoutRoute = [{ path: ':dcvid', component: NewWordComponent }];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class NewWordRoutingModule {}
