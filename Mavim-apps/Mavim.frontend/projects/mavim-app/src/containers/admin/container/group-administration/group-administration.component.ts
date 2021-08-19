import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { RoutingFacade } from '../../../../shared/router/service';
import { GroupFacade } from './service/group.facade';

@Component({
	selector: 'mav-group-administration',
	templateUrl: './group-administration.component.html',
	styleUrls: ['./group-administration.component.scss']
})
export class GroupAdministrationComponent {
	public constructor(
		private readonly routingFacade: RoutingFacade,
		private readonly router: Router,
		public readonly groupsFacade: GroupFacade
	) {}

	public showGroupSplitScreen = false;
	public navigateToCreateGroup(): void {
		this.router.navigateByUrl(`/admin/groups/add`);
	}

	public backToHome(): void {
		this.routingFacade.home(undefined);
	}

	public onActivate(): void {
		this.showGroupSplitScreen = true;
	}

	public onDeactivate(): void {
		this.showGroupSplitScreen = false;
	}
}
