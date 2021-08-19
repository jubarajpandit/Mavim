import { Component, Input } from '@angular/core';

@Component({
	selector: 'mav-o365-logo',
	templateUrl: './o365-logo.component.html',
	styleUrls: ['./o365-logo.component.scss']
})
export class O365LogoComponent {
	@Input() public img = 'assets/images/logo_mavim_white_new.png';
	@Input() public title = 'ngStyleGuide';
	@Input() public url = 'https://www.mavim.nl';
}
