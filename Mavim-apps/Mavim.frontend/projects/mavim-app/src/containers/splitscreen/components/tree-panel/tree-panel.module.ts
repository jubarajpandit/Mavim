import { NgModule } from '@angular/core';
import { TopicTreeFacade } from './services/topic-tree.facade';
import { Store, StoreModule } from '@ngrx/store';
import { CommonModule } from '@angular/common';
import { EffectsModule } from '@ngrx/effects';
import { TreeEffect } from './+state/effects/tree.effect';
import { treeReducer } from './+state/reducers/tree.reducer';
import { namespace } from './+state';
import { LoadersModule } from '../../../../shared/loaders/loaders.module';
import { GenericTreeFacade } from '../../../../shared/tree/services/generictree.facade';
import { ModalModule } from '../../../../shared/modal/modal.module';
import { TreeContainerModule } from '../../../tree/tree-container.module';
import { TopicModule } from '../../../../shared/topic/topic.module';
import { TreeEditEffects } from '../../../../shared/tree/+state';

export function treeFacadeFactory(store: Store): GenericTreeFacade {
	return new TopicTreeFacade(store);
}

@NgModule({
	imports: [
		CommonModule,
		TreeContainerModule,
		TopicModule,
		LoadersModule,
		ModalModule,
		StoreModule.forFeature(namespace, treeReducer),
		EffectsModule.forFeature([TreeEffect]),
		EffectsModule.forFeature([TreeEditEffects])
	],
	exports: [],
	providers: [TopicTreeFacade]
})
export class TreePanelModule {}
