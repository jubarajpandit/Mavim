import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InitComponent } from './components/init.component';
import { InitRoutingModule } from './init-routing.module';
import { LanguageModule } from '../../shared/language/language-module';

@NgModule({
	declarations: [InitComponent],
	imports: [CommonModule, InitRoutingModule, LanguageModule],
	exports: [],
	providers: []
})
export class InitModule {}
