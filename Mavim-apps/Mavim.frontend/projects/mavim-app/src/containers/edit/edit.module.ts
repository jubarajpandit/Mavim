import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditRoutingModule } from './edit-routing.module';
import { EditPanelComponent } from './components/edit-panel/edit-panel.component';
import { ModalModule } from '../../shared/modal/modal.module';
import { FieldModule } from '../../shared/field/field.module';
import { TopicModule } from '../../shared/topic/topic.module';
import { RelationModule } from '../../shared/relation/relation.module';
import { ComponentsModule } from '../../shared/components/components.module';
import { WopiModule } from '../../shared/wopi/wopi.module';
import { LoadersModule } from '../../shared/loaders/loaders.module';
import { FeatureflagModule } from '../../shared/featureflag/featureflag.module';

@NgModule({
	declarations: [EditPanelComponent],
	imports: [
		CommonModule,
		EditRoutingModule,
		ModalModule,
		FieldModule,
		TopicModule,
		WopiModule,
		LoadersModule,
		ComponentsModule,
		RelationModule,
		FeatureflagModule
	],
	exports: [],
	providers: []
})
export class EditContainerModule {}
