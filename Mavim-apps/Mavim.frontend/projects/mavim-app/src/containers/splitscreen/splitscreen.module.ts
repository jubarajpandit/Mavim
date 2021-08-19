import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SplitScreenComponent } from './components/splitscreen/splitscreen.component';
import { SplitScreenRoutingModule } from './splitscreen-routing.module';
import { StoreModule } from '@ngrx/store';
import { splitScreenReducer } from './+state/reducers/splitscreen.reducers';
import { SplitScreenFacade } from './service/splitscreen.facade';
import { TopicContainerModule } from '../topic/topic.module';
import { ComponentsModule } from '../../shared/components/components.module';
import { TreePanelComponent } from './components/tree-panel/tree-panel.component';
import { SidebarButtonComponent } from './components/sidebar-button/sidebar-button.component';
import { RoutingModule } from '../../shared/router/router.module';
import { TopicFacade } from '../../shared/topic/services/topic.facade';
import { RoutingBrowserService } from '../../shared/router/service/routing-browser.service';
import { LoadersModule } from '../../shared/loaders/loaders.module';
import { TreePanelModule } from './components/tree-panel/tree-panel.module';
import { TreeContainerModule } from '../tree/tree-container.module';
import { FeatureflagModule } from '../../shared/featureflag/featureflag.module';
@NgModule({
	declarations: [
		SplitScreenComponent,
		TreePanelComponent,
		SidebarButtonComponent
	],
	imports: [
		CommonModule,
		TopicContainerModule,
		ComponentsModule,
		FeatureflagModule,
		SplitScreenRoutingModule,
		LoadersModule,
		TreeContainerModule,
		RoutingModule.forChild(),
		StoreModule.forFeature('splitScreen', splitScreenReducer),
		TreePanelModule
	],
	providers: [SplitScreenFacade, TopicFacade, RoutingBrowserService]
})
export class SplitScreenModule {}
