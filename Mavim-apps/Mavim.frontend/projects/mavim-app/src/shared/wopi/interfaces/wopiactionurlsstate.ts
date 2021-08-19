import { FetchStatus } from '../../enums/FetchState';
import { WopiActionUrls } from '../models/wopi-actionurls.model';

export interface WopiActionUrlsState {
	wopiActionUrls: WopiActionUrls;
	fetchWopiActionUrls: FetchStatus;
}
