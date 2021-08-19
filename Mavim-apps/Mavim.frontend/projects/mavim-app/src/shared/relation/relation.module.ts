import { NgModule } from '@angular/core';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { RelationEffects } from './+state/effects/relation.effects';
import { reducer } from './+state/reducers/relation.reducers';
import { RelationsComponent } from './components/relations/relations.component';
import { RelationsEditComponent } from './components/relations-edit/relations-edit.component';
import { CommonModule } from '@angular/common';
import { RelationsCreateComponent } from './components/relations-create/relations-create.component';
import { LoadersModule } from '../loaders/loaders.module';
import { ComponentsModule } from '../components/components.module';
import { RelationsCreateModule } from './components/relations-create/relations-create.module';
import { TreeContainerModule } from '../../containers/tree/tree-container.module';
import { FeatureflagModule } from '../featureflag/featureflag.module';

const RelationModuleComponents = [
	RelationsComponent,
	RelationsCreateComponent,
	RelationsEditComponent
];

@NgModule({
	imports: [
		CommonModule,
		StoreModule.forFeature('relations', reducer),
		EffectsModule.forFeature([RelationEffects]),
		TreeContainerModule,
		LoadersModule,
		ComponentsModule,
		RelationsCreateModule,
		FeatureflagModule
	],
	exports: [...RelationModuleComponents],
	providers: [],
	declarations: [...RelationModuleComponents]
})
export class RelationModule {}
