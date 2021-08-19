import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconAddGroupComponent } from './add-group-icon/add-group-icon.component';

const components = [IconAddGroupComponent];
@NgModule({
	declarations: [...components],
	imports: [CommonModule],
	exports: [...components],
	providers: []
})
export class IconsModule {}
