import { Component, Input, HostBinding } from '@angular/core';

@Component({
	selector: 'mav-button',
	templateUrl: './button.component.html',
	styleUrls: ['./button.component.scss']
})
export class ButtonComponent {
	@Input() public text = 'button';
	@Input() public cssClass = '';
	@HostBinding('class.pe-none') @Input() public disabled = false;
}
