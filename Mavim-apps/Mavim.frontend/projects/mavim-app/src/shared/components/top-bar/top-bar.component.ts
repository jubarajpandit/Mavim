import { Component, Output, EventEmitter } from '@angular/core';

@Component({
	selector: 'mav-top-bar',
	templateUrl: './top-bar.component.html',
	styleUrls: ['./top-bar.component.scss']
})
export class TopBarComponent {
	@Output() public toggle = new EventEmitter();

	public toggleNav(): void {
		this.toggle.emit();
	}
}
