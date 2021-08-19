import { NgModule } from '@angular/core';

import { CommonModule } from '@angular/common';
import { RelationTreeFacade } from './services/relationship-tree.facade';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { namespace } from './+state';
import { relationshipTreeReducer } from './+state/reducers/relationship-tree.reducer';
import { RelationshipTreeEffect } from './+state/effects/relationship-tree.effect';

@NgModule({
	imports: [
		CommonModule,
		StoreModule.forFeature(namespace, relationshipTreeReducer),
		EffectsModule.forFeature([RelationshipTreeEffect])
	],
	exports: [],
	providers: [RelationTreeFacade]
})
export class RelationsCreateModule {}
