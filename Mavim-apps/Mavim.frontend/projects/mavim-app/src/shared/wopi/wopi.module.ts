import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WopiTestViewerComponent } from './components/test-viewer/test-viewer.component';
import { WordViewerComponent } from './components/word-viewer/word-viewer.component';
import { WordEditorComponent } from './components/word-editor/word-editor.component';
import { ChartViewerComponent } from './components/chart-viewer/chart-viewer.component';
import { FormsModule } from '@angular/forms';
import { WopiFacade } from './services/wopi.facade';
import { EffectsModule } from '@ngrx/effects';
import { WopiEffects } from './+state/effects/wopi.effects';
import { StoreModule } from '@ngrx/store';
import {
	wopiReducer,
	wopiActionUrlsReducer
} from './+state/reducers/wopi.reducers';
import { ChartViewerWithButtonsComponent } from './components/chart-viewer-with-buttons/chart-viewer-with-buttons.component';

const wopiComponents = [
	WopiTestViewerComponent,
	WordViewerComponent,
	WordEditorComponent,
	ChartViewerComponent,
	ChartViewerWithButtonsComponent
];

@NgModule({
	declarations: [...wopiComponents],
	imports: [
		CommonModule,
		FormsModule,
		StoreModule.forFeature('wopi', wopiReducer),
		StoreModule.forFeature('wopiactionurls', wopiActionUrlsReducer),
		EffectsModule.forFeature([WopiEffects])
	],
	exports: [...wopiComponents],
	providers: [WopiFacade]
})
export class WopiModule {}
