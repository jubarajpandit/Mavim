import { NgModule } from '@angular/core';
import { TreeContainerComponent } from './tree-container.component';
import { TreeModule } from '../../shared/tree/tree.module';
import { CommonModule } from '@angular/common';
import { LoadersModule } from '../../shared/loaders/loaders.module';
import { ComponentsModule } from '../../shared/components/components.module';

@NgModule({
	declarations: [TreeContainerComponent],
	imports: [CommonModule, TreeModule, LoadersModule, ComponentsModule],
	exports: [TreeContainerComponent],
	providers: []
})
export class TreeContainerModule {}
