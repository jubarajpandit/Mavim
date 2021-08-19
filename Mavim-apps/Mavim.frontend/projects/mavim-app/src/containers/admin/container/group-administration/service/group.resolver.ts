import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { GroupFacade } from './group.facade';

@Injectable({ providedIn: 'root' })
export class GroupsResolver implements Resolve<boolean> {
	public constructor(private readonly groupFacade: GroupFacade) {}

	public resolve(): boolean {
		this.groupFacade.loadGroups();
		return true;
	}
}
