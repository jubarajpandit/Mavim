import { Component, OnInit, Input } from '@angular/core';

interface IButtonIcons {
	type: string;
	icon: string;
	color: string;
}

export const buttonIcons: IButtonIcons[] = [
	{ type: 'user', icon: 'mdl2 mdl2-member', color: 'btn-info' },
	{ type: 'group', icon: 'mdl2 mdl2-group', color: 'btn-info' },
	{ type: 'team', icon: 'mdl2 mdl2-people', color: 'btn-info' },
	{ type: 'add', icon: 'mdl2 mdl2-add', color: 'btn-new' },
	{ type: 'close', icon: 'mdl2 mdl2-2-ChromeClose', color: 'btn-collapse' },
	{ type: 'copy', icon: 'mdl2 mdl2-copy', color: 'btn-action' }
];

@Component({
	selector: 'mav-icon-button',
	templateUrl: './icon-button.component.html',
	styleUrls: ['./icon-button.component.scss']
})
export class IconButtonComponent implements OnInit {
	public btnIcon: string;
	@Input() public label = 'add';

	@Input() public type = 'add';

	public getButtonClass(icons: IButtonIcons[], iconType: string): string {
		const buttonType = icons.find((btnIcn) => btnIcn.type === iconType);
		const icon = buttonType ? buttonType.icon : '';
		const color = buttonType ? buttonType.color : 'info';
		const classNames = `${color} ${icon}`;
		return classNames;
	}

	public ngOnInit(): void {
		this.btnIcon = this.getButtonClass(buttonIcons, this.type);
	}
}
