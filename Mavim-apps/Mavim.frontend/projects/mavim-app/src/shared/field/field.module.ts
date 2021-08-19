import { NgModule } from '@angular/core';
import { StoreModule, Store } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { FieldEffects } from './+state/effects/field.effects';
import { reducer } from './+state/reducers/field.reducers';
import { FieldsComponent } from './components/fields/fields.component';
import { FieldsEditComponent } from './components/fields-edit/fields-edit.component';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';
import { EditPanelFacade } from './services/editpanelfield.facade';
import { FieldService } from './services/field.service';
import * as fromFieldState from './+state/reducers/field.reducers';
import { EDITPANEL_FACADE } from '../../containers/edit/components/tokens/edit-panel.token';
import { ModalModule } from '../modal/modal.module';
import { ComponentsModule } from '../components/components.module';
import { FieldEditRelationModule } from './components/fields-edit-relation/field-edit-relation.module';
import { LoadersModule } from '../loaders/loaders.module';

const FieldModuleComponents = [FieldsComponent, FieldsEditComponent];

export function editPanelFacade(
	fieldService: FieldService,
	fieldStore: Store<fromFieldState.FeatureState>
): EditPanelFacade {
	return new EditPanelFacade(fieldService, fieldStore);
}

@NgModule({
	imports: [
		CommonModule,
		StoreModule.forFeature('fields', reducer),
		EffectsModule.forFeature([FieldEffects]),
		FieldEditRelationModule,
		FormsModule,
		LoadersModule,
		ComponentsModule,
		ModalModule
	],
	exports: [...FieldModuleComponents],
	providers: [
		{
			provide: EDITPANEL_FACADE,
			useFactory: editPanelFacade,
			deps: [FieldService, Store]
		}
	],
	declarations: [...FieldModuleComponents]
})
export class FieldModule {}
