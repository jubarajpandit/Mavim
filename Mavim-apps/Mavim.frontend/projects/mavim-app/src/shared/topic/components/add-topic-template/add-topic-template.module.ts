import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComponentsModule } from '../../../components/components.module';
import { ReactiveFormsModule } from '@angular/forms';
import { AddTopicTemplateComponent } from './add-topic-template.component';
import { AddChildTopicTemplateComponent } from './add-child-topic-template.component';
import { LoadersModule } from '../../../loaders/loaders.module';

const components = [AddTopicTemplateComponent, AddChildTopicTemplateComponent];
@NgModule({
	declarations: [...components],
	imports: [
		CommonModule,
		ComponentsModule,
		LoadersModule,
		ReactiveFormsModule
	],
	exports: [...components],
	providers: []
})
export class AddTopicTemplateModule {}
