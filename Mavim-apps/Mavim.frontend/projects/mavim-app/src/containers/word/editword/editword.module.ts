import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditWordRoutingModule } from './editword-routing.module';
import { EditWordComponent } from '../editword/components/edit-panel/edit-word.component';
import { WopiModule } from '../../../shared/wopi/wopi.module';
import { LoadersModule } from '../../../shared/loaders/loaders.module';

@NgModule({
	declarations: [EditWordComponent],
	imports: [CommonModule, EditWordRoutingModule, WopiModule, LoadersModule],
	exports: [],
	providers: []
})
export class EditWordContainerModule {}
