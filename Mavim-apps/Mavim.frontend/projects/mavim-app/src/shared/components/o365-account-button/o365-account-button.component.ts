import { Component, Input } from '@angular/core';
@Component({
	selector: 'mav-o365-account-button',
	templateUrl: './o365-account-button.component.html',
	styleUrls: ['./o365-account-button.component.scss']
})
export class O365AccountButtonComponent {
	@Input() public img = '../../../assets/images/person_placeholder.png';
	@Input() public userName = 'Unknown';

	public click(): void {
		// TODO: Do we need to log here? Or what do we do? (WI14764)
	}

	public logout(): void {
		// TODO: Do we need to log here? Or what do we do? (WI14764)
	}

	public updateUrl(): void {
		// TODO: Do we need to log here? Or what do we do? (WI14764)
		this.img = '../../../assets/images/person_placeholder.png';
	}
}
