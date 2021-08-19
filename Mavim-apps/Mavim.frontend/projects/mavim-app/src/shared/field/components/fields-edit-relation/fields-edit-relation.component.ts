import {
	Component,
	OnInit,
	Output,
	EventEmitter,
	OnDestroy,
	Input
} from '@angular/core';
import { Topic } from '../../../../shared/topic/models/topic.model';
import { Subject } from 'rxjs';
import { RelationTreeFacade } from './services/relationship-tree.facade';
import { TopicFacade } from '../../../topic/services/topic.facade';
import { take, filter, takeWhile, takeUntil } from 'rxjs/operators';
import { FetchStatus } from '../../../enums/FetchState';

@Component({
	selector: 'mav-fields-edit-relation',
	templateUrl: './fields-edit-relation.component.html',
	styleUrls: ['./fields-edit-relation.component.scss']
})
export class FieldsEditRelationComponent implements OnInit, OnDestroy {
	public constructor(
		public readonly treeFacade: RelationTreeFacade,
		public readonly topicFacade: TopicFacade
	) {}

	@Output() public relationFieldChanged = new EventEmitter<Topic>();
	@Output() public closeComponent = new EventEmitter<boolean>();
	@Input() public initLocation: string;

	public saveButtonClass = 'save hidden';
	public closeButtonClass = 'cancel';

	// TODO: Move to translations
	public saveButtonText = 'Add';
	public cancelButtonText = 'Cancel';

	private readonly destroySubject$: Subject<void> = new Subject();
	private selectedTopic: Topic;

	public ngOnInit(): void {
		if (this.initLocation) {
			this.loadInitLocation();
		} else {
			this.openDefaultLocation();
		}
	}

	public ngOnDestroy(): void {
		this.destroySubject$.next();
		this.destroySubject$.complete();
		this.treeFacade.ClearTree();
	}

	public onTopicSelected(topicId: string): void {
		this.topicFacade
			.getTopicByDcv(topicId)
			.pipe(take(1))
			.subscribe((topic) => {
				this.selectedTopic = topic;
				this.saveButtonClass = 'save show';
			});
	}

	public close(): void {
		this.closeComponent.emit(false);
	}

	public saveRelationField(): void {
		if (this.selectedTopic?.dcv === this.initLocation) {
			this.closeComponent.emit(true);
		} else {
			this.relationFieldChanged.emit(this.selectedTopic);
		}
	}

	private openDefaultLocation(): void {
		this.topicFacade.getCategories
			.pipe(
				filter((topics) => !!topics && topics.length > 0),
				take(1)
			)
			.subscribe((topics) => {
				const withWhat = topics.find(
					(topic) => topic.name === 'With What'
				);
				this.treeFacade.ExpandTo(withWhat.dcv);
			});
	}

	private loadInitLocation(): void {
		this.treeFacade.ExpandTo(this.initLocation);

		const test = new Subject();
		this.treeFacade.selectFetchStatus
			.pipe(
				takeWhile((fetchStatus) => fetchStatus !== FetchStatus.Fetched),
				takeUntil(test)
			)
			.subscribe((fetchStatus) => {
				if (fetchStatus === FetchStatus.Failed) {
					this.openDefaultLocation();
					test.next();
					test.complete();
				}
			});
	}
}
