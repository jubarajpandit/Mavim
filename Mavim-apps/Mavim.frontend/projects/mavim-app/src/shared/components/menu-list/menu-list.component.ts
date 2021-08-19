import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ListItem } from './models/list-item.model';

@Component({
	selector: 'mav-menu-list',
	templateUrl: './menu-list.component.html',
	styleUrls: ['./menu-list.component.scss']
})
export class MenuListComponent {
	@Input() public listItems: ListItem[] = [];
	@Output() public itemClicked = new EventEmitter<string>();

	public onClick(name: string): void {
		this.itemClicked.emit(name);
	}
}
