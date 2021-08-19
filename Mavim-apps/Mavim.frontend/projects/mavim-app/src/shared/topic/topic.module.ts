import { NgModule } from '@angular/core';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { TopicEffects } from './+state/effects/topic.effects';
import { topicReducer } from './+state/reducers/topic.reducers';
import { AddTopicTemplateModule } from './components/add-topic-template/add-topic-template.module';
import { DeleteTopicTemplateModule } from './components/delete-topic-template/delete-topic-template.module';

@NgModule({
	declarations: [],
	imports: [
		AddTopicTemplateModule,
		DeleteTopicTemplateModule,
		StoreModule.forFeature('topics', topicReducer),
		EffectsModule.forFeature([TopicEffects])
	],
	providers: [],
	exports: []
})
export class TopicModule {}
