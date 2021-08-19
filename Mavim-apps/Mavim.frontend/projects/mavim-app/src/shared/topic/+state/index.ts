import { RootState } from '../../../app/+state';
import { TopicState } from '../interfaces/topic-state.interface';

export interface State extends RootState {
	topics: TopicState;
}
