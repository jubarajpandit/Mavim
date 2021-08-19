import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { relationshipTreeReducer as reducer } from './+state/reducers/relationship-tree.reducer';
import { RelationEffects } from '../../../relation/+state/effects/relation.effects';
import { FieldsEditRelationComponent } from './fields-edit-relation.component';
import { RelationTreeFacade } from './services/relationship-tree.facade';
import { ComponentsModule } from '../../../components/components.module';
import { TreeContainerModule } from '../../../../containers/tree/tree-container.module';
import { RelationshipTreeEffect } from './+state/effects/relationship-tree.effect';
import { namespace } from './+state';

@NgModule({
	declarations: [FieldsEditRelationComponent],
	imports: [
		CommonModule,
		StoreModule.forFeature(namespace, reducer),
		EffectsModule.forFeature([RelationEffects, RelationshipTreeEffect]),
		ComponentsModule,
		TreeContainerModule
	],
	exports: [FieldsEditRelationComponent],
	providers: [RelationTreeFacade]
})
export class FieldEditRelationModule {}
