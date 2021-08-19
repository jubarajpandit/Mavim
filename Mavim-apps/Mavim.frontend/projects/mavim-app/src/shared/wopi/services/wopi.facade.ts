import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import * as fromWopi from '../+state';
import { Store } from '@ngrx/store';
import {
	selectWopiByDcv,
	selectWopiActionUrls,
	selectFetchWopiActionUrls
} from '../+state/selectors/wopi.selectors';
import { Wopi } from '../models/wopi.model';
import { WopiActionUrls } from '../models/wopi-actionurls.model';
import { FetchStatus } from '../../enums/FetchState';

@Injectable()
export class WopiFacade {
	public constructor(private readonly store: Store) {}

	public getWopiActionUrls(): Observable<WopiActionUrls> {
		return this.store.select(selectWopiActionUrls);
	}
	public getFetchWopiActionUrls(): Observable<FetchStatus> {
		return this.store.select(selectFetchWopiActionUrls);
	}

	public getFileInfo(dcv: string): Observable<Wopi> {
		return this.store.select(selectWopiByDcv(dcv));
	}

	public loadFileInfo(dcv: string): void {
		this.store.dispatch(new fromWopi.LoadFileInfo(dcv));
	}

	public loadWopiActionUrls(): void {
		this.store.dispatch(new fromWopi.LoadWopiActionUrls());
	}
}
