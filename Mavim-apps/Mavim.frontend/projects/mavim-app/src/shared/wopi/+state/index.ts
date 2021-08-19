import { WopiActionUrls } from './../models/wopi-actionurls.model';
import { RootState } from '../../../app/+state';
import { EntityState } from '@ngrx/entity';
import { Wopi } from '../models/wopi.model';

export * from './actions/wopi.actions';
export * from './reducers/wopi.reducers';

export interface State extends RootState {
	wopi: EntityState<Wopi>;
	wopiactionurls: EntityState<WopiActionUrls>;
}
