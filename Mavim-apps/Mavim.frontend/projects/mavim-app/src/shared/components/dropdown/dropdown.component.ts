import { Component, OnInit, Input } from '@angular/core';

@Component({
	selector: 'mav-dropdown',
	templateUrl: './dropdown.component.html',
	styleUrls: ['./dropdown.component.scss']
})
export class DropdownComponent implements OnInit {
	@Input() public header: string;
	@Input() public typeClass: string;

	public ngOnInit(): void {
		this.typeClass = this.typeClass ? `dropdown--${this.typeClass}` : '';
	}
}
