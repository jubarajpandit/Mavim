import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PortalModule } from '@angular/cdk/portal';
import { ModalModule } from '../../modal.module';
import { ComponentsModule } from '../../../components/components.module';
import { ModalFactoryComponent } from './modalfactory.component';

@NgModule({
	declarations: [ModalFactoryComponent],
	imports: [CommonModule, PortalModule, ModalModule, ComponentsModule],
	exports: [ModalFactoryComponent]
})
export class ModalFactoryModule {}
