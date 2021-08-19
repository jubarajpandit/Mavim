import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewWordRoutingModule } from './newword-routing.module';
import { NewWordComponent } from './components/new-word.component';
import { WopiModule } from '../../../shared/wopi/wopi.module';
import { LoadersModule } from '../../../shared/loaders/loaders.module';

@NgModule({
	declarations: [NewWordComponent],
	imports: [CommonModule, NewWordRoutingModule, WopiModule, LoadersModule],
	exports: [],
	providers: []
})
export class NewWordContainerModule {}
