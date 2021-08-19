import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TopicComponent } from './components/topic/topic.component';
import { LoadersModule } from '../../shared/loaders/loaders.module';
import { FieldModule } from '../../shared/field/field.module';
import { RelationModule } from '../../shared/relation/relation.module';
import { TreeModule } from '../../shared/tree/tree.module';
import { NotificationsModule } from '../../shared/notification/notification.module';
import { TopicModule } from '../../shared/topic/topic.module';
import { ComponentsModule } from '../../shared/components/components.module';
import { WopiModule } from '../../shared/wopi/wopi.module';
import { ChartModule } from '../../shared/chart/chart.module';
import { FeatureflagModule } from '../../shared/featureflag/featureflag.module';

@NgModule({
	declarations: [TopicComponent],
	imports: [
		CommonModule,
		ComponentsModule,
		LoadersModule,
		TopicModule,
		FieldModule,
		ChartModule,
		RelationModule,
		WopiModule,
		TreeModule,
		NotificationsModule,
		FeatureflagModule
	],
	exports: [TopicComponent]
})
export class TopicContainerModule {}
