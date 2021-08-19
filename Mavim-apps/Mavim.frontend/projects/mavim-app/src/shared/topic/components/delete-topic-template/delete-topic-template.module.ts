import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComponentsModule } from '../../../components/components.module';
import { DeleteTopicTemplateComponent } from './delete-topic-template.component';

@NgModule({
	declarations: [DeleteTopicTemplateComponent],
	imports: [CommonModule, ComponentsModule],
	exports: [DeleteTopicTemplateComponent],
	providers: []
})
export class DeleteTopicTemplateModule {}
