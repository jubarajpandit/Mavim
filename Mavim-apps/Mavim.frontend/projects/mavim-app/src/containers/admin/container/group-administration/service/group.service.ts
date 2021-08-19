import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Group } from '../model/group';

@Injectable({
	providedIn: 'root'
})
export class GroupService {
	public constructor(private http: HttpClient) {}
	private readonly groupsApiUrl =
		'assets/group-overview/group-overview-sample.json';

	public getGroups(): Observable<Group[]> {
		return this.http.get<Group[]>(this.groupsApiUrl);
	}
}
