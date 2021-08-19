import { Component, OnInit, Input } from '@angular/core';

@Component({
	selector: 'mav-menu-button',
	templateUrl: './menu-button.component.html',
	styleUrls: ['./menu-button.component.scss']
})
export class MenuButtonComponent implements OnInit {
	public class = 'mdl2-menu';
	@Input() public global = false;

	public click(): void {
		// TODO: Do we need to log here? (WI14764)
	}

	public ngOnInit(): void {
		this.class = this.global ? 'mdl2-home' : 'mdl2-menu';
	}
}
