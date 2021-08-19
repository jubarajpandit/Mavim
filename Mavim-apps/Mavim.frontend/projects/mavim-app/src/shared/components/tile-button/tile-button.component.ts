import { Component, Input } from '@angular/core';

@Component({
	selector: 'mav-tile-button',
	templateUrl: './tile-button.component.html',
	styleUrls: ['./tile-button.component.scss']
})
export class TileButtonComponent {
	@Input() public toolTipFeatureFlag = false;
	@Input() public text = '';
	@Input() public iconClass = '';
	@Input() public divClass = '';
}
