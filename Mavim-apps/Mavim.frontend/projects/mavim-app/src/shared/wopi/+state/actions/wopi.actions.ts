import { Action } from '@ngrx/store';
import { Wopi } from '../../models/wopi.model';
import { WopiActionUrls } from '../../models/wopi-actionurls.model';

export enum WopiActionTypes {
	LoadFileInfo = '[Wopi] Get file info',
	LoadFileInfoFail = '[Wopi] Get file info fail',
	LoadFileInfoSuccess = '[Wopi] Get file info success',
	LoadWopiActionUrls = '[LoadWopiActionUrls] Get WopiActionUrls',
	LoadWopiActionUrlsFail = '[LoadWopiActionUrls] Get WopiActionUrls fail',
	LoadWopiActionUrlsSuccess = '[LoadWopiActionUrls] Get WopiActionUrls success'
}

export class LoadFileInfo implements Action {
	public constructor(public payload: string) {}
	public readonly type = WopiActionTypes.LoadFileInfo;
}

export class LoadFileInfoFail implements Action {
	public constructor(public payload: Wopi) {}
	public readonly type = WopiActionTypes.LoadFileInfoFail;
}

export class LoadFileInfoSuccess implements Action {
	public constructor(public payload: Wopi) {}
	public readonly type = WopiActionTypes.LoadFileInfoSuccess;
}

export class LoadWopiActionUrls implements Action {
	public readonly type = WopiActionTypes.LoadWopiActionUrls;
}

export class LoadWopiActionUrlsFail implements Action {
	public constructor(public payload: WopiActionUrls) {}
	public readonly type = WopiActionTypes.LoadWopiActionUrlsFail;
}

export class LoadWopiActionUrlsSuccess implements Action {
	public constructor(public payload: WopiActionUrls) {}
	public readonly type = WopiActionTypes.LoadWopiActionUrlsSuccess;
}

export type WopiActions =
	| LoadFileInfo
	| LoadFileInfoFail
	| LoadFileInfoSuccess
	| LoadWopiActionUrls
	| LoadWopiActionUrlsFail
	| LoadWopiActionUrlsSuccess;
