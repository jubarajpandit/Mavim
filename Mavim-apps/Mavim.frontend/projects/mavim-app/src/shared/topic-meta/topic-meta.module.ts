import { NgModule } from '@angular/core';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { topicMetaReducer } from './+state/reducers/topic-meta.reducer';
import { TopicMetaEffects } from './+state/effects/topic-meta.effects';

@NgModule({
	declarations: [],
	imports: [
		StoreModule.forFeature('topicMeta', topicMetaReducer),
		EffectsModule.forFeature([TopicMetaEffects])
	],
	providers: [],
	exports: []
})
export class TopicMetaModule {}
