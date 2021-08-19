import { Component, Input } from '@angular/core';

@Component({
	selector: 'mav-logo',
	templateUrl: './logo.component.html',
	styleUrls: ['./logo.component.scss']
})
export class LogoComponent {
	@Input() public by = false;
	@Input() public byText: string;
	@Input() public img = '../../../images/logo_mavim160x58.jpg';
	@Input() public url = 'https://www.mavim.nl';
}
