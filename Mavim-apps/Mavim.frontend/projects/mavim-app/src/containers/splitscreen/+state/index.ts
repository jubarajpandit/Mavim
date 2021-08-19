import { RootState } from '../../../app/+state';
import { SplitScreenState } from '../interfaces/splitscreen-state.interface';

export * from './actions/splitscreen.actions';
export * from './reducers/splitscreen.reducers';

export interface State extends RootState {
	splitScreen: SplitScreenState;
}
