import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DropdownComponent } from './dropdown/dropdown.component';
import { IconButtonComponent } from './icon-button/icon-button.component';
import { LogoComponent } from './logo/logo.component';
import { MenuButtonComponent } from './menu-button/menu-button.component';
import { O365AccountButtonComponent } from './o365-account-button/o365-account-button.component';
import { O365ButtonComponent } from './o365-button/o365-button.component';
import { O365LogoComponent } from './o365-logo/o365-logo.component';
import { O365navbarComponent } from './o365navbar/o365navbar.component';
import { PrimaryButtonComponent } from './primary-button/primary-button.component';
import { TopBarComponent } from './top-bar/top-bar.component';
import { TopicEditNameComponent } from './topic-edit-name/topic-edit-name.component';
import { TopicChildrenComponent } from './topic-children/topic-children.component';
import { TopicIconComponent } from './topic-icon/topic-icon.component';
import { ButtonComponent } from './button/button.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MenuListComponent } from './menu-list/menu-list.component';
import { SelectDropdownComponent } from './select-dropdown/select-dropdown.component';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { UppercaseFirstCharacterPipe } from '../pipes/UppercaseFirstCharacter.pipe';
import { SecurePipe } from '../pipes/Secure.pipe';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { ResourceNotFoundComponent } from './resource-not-found/resource-not-found.component';
import { TileButtonComponent } from './tile-button/tile-button.component';
import { TooltipModule } from '../tooltip/tooltip.module';
import { TopicCustomIconComponent } from './topic-custom-icon/topic-custom-icon.component';

const components = [
	DropdownComponent,
	IconButtonComponent,
	LogoComponent,
	MenuButtonComponent,
	MenuListComponent,
	O365AccountButtonComponent,
	O365ButtonComponent,
	O365LogoComponent,
	O365navbarComponent,
	PrimaryButtonComponent,
	TopBarComponent,
	TopicEditNameComponent,
	TopicChildrenComponent,
	TopicIconComponent,
	ButtonComponent,
	SelectDropdownComponent,
	PageNotFoundComponent,
	ResourceNotFoundComponent,
	TileButtonComponent,
	TopicCustomIconComponent
];

const pipes = [UppercaseFirstCharacterPipe];
const secure = [SecurePipe];

@NgModule({
	declarations: [components, pipes, secure],
	imports: [
		CommonModule,
		FormsModule,
		ReactiveFormsModule,
		ScrollingModule,
		TooltipModule
	],
	exports: [components],
	providers: []
})
export class ComponentsModule {}
