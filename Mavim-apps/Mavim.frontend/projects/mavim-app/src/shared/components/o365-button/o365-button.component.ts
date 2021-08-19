import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
// icon index
export const o365Icons = [
	{ type: 'settings', icon: 'settings' },
	{ type: 'dots', icon: 'menu' },
	{ type: 'help', icon: 'questionmark' },
	{ type: 'menu', icon: 'GlobalNavButton' },
	{ type: 'close', icon: '2-ChromeClose' },
	{ type: 'apps', icon: 'all-apps' }
];

@Component({
	selector: 'mav-o365-button',
	templateUrl: './o365-button.component.html',
	styleUrls: ['./o365-button.component.scss']
})
export class O365ButtonComponent implements OnInit {
	@Output() public action = new EventEmitter();
	public icon: string;

	@Input() public type: string;

	public blur(): void {
		// TODO: Do we need to log here? Or what do we do? (WI14764)
	}

	public click(): void {
		// TODO: Do we need to log here? Or what do we do? (WI14764)
	}

	public ngOnInit(): void {
		// eslint-disable-next-line
    const o365type = o365Icons.find((type) => type.type === this.type);
		this.icon = o365type ? o365type.icon : 'menu';
	}
}
