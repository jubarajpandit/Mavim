import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PopperDirective } from '../../shared/tooltip/poper/popper.directive';

@NgModule({
	declarations: [PopperDirective],
	imports: [CommonModule],
	exports: [PopperDirective],
	providers: []
})
export class TooltipModule {}
