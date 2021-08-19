import { Component, Input, Output } from '@angular/core';
import { EventEmitter } from 'events';

@Component({
	selector: 'mav-primary-button',
	templateUrl: './primary-button.component.html',
	styleUrls: ['./primary-button.component.scss']
})
export class PrimaryButtonComponent {
	public constructor() {
		this.text = 'unit';
	}
	@Input() public class = 'btn-primary';
	@Output() public clickButton = new EventEmitter();
	@Input() public text = 'Primary button';
}
