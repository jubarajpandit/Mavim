import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { WopiTestComponent } from './components/wopi-panel/wopi-test.component';
import { LayoutRoute } from '../../shared/layout/interfaces';

const routes: LayoutRoute = [{ path: ':dcvid', component: WopiTestComponent }];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class WopiTestRoutingModule {}
