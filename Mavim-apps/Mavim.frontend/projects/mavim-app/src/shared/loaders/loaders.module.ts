import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DotLoaderComponent } from './dot-loader/dot-loader.component';
import { SpinLoaderComponent } from './spin-loader/spin-loader.component';

@NgModule({
	declarations: [DotLoaderComponent, SpinLoaderComponent],
	imports: [CommonModule],
	exports: [DotLoaderComponent, SpinLoaderComponent]
})
export class LoadersModule {}
