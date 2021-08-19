import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { EditWordComponent } from '../editword/components/edit-panel/edit-word.component';
import { LayoutRoute } from '../../../shared/layout/interfaces';

const routes: LayoutRoute = [{ path: ':dcvid', component: EditWordComponent }];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class EditWordRoutingModule {}
