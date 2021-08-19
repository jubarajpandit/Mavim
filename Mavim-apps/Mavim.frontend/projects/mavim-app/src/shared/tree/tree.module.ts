import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadersModule } from '../loaders/loaders.module';
import { ComponentsModule } from '../components/components.module';
import { TreeCdkComponent } from './components/tree-cdk/tree-cdk.component';
import { CdkTreeModule } from '@angular/cdk/tree';
import { FeatureflagModule } from '../featureflag/featureflag.module';

const components = [TreeCdkComponent];
@NgModule({
	declarations: components,
	imports: [
		CommonModule,
		LoadersModule,
		ComponentsModule,
		CdkTreeModule,
		FeatureflagModule
	],
	exports: components
})
export class TreeModule {}
