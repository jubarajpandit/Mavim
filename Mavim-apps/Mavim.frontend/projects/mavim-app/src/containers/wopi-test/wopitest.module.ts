import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WopiTestRoutingModule } from './wopitest-routing.module';
import { WopiTestComponent } from './components/wopi-panel/wopi-test.component';
import { WopiModule } from '../../shared/wopi/wopi.module';
import { LoadersModule } from '../../shared/loaders/loaders.module';

@NgModule({
	declarations: [WopiTestComponent],
	imports: [CommonModule, WopiTestRoutingModule, WopiModule, LoadersModule],
	exports: [],
	providers: []
})
export class WopiTestContainerModule {}
