import {
	Component,
	OnInit,
	OnDestroy,
	Input,
	Output,
	EventEmitter
} from '@angular/core';
import { selectCategories } from '../../../../shared/topic/+state/selectors/topic.selector';
import { Topic } from '../../../../shared/topic/models/topic.model';
import { selectTopicById } from '../../../../shared/topic/+state/selectors/topic.selector';
import { Store } from '@ngrx/store';
import { Subject } from 'rxjs';
import { EditStatus } from '../../../../containers/edit/enums/edit-status.enum';
import { EditRelation } from '../../models/edit-relation.model';
import { takeUntil, take } from 'rxjs/operators';
import { RelationTopic } from '../../models/relation-topic.model';
import { RelationTreeFacade } from './services/relationship-tree.facade';
import { DropdownList } from './models/dropdownlist';

@Component({
	selector: 'mav-relations-create',
	templateUrl: './relations-create.component.html',
	styleUrls: ['./relations-create.component.scss']
})
export class RelationsCreateComponent implements OnInit, OnDestroy {
	public constructor(
		private readonly store: Store,
		public readonly treeFacade: RelationTreeFacade
	) {}

	@Input() public sourceDcv: string;
	@Output() public relationCreated = new EventEmitter<EditRelation>();
	@Output() public cancelComponent = new EventEmitter<boolean>();

	public closeButtonClass = 'cancel';

	// TODO: Move to translations
	public saveButtonText = 'Add';
	public cancelButtonText = 'Cancel';

	// dev express relation catagories
	public relationCategoriesItem: DropdownList[];
	public selectedRelationship: string = undefined;

	private readonly destroySubject$: Subject<void> = new Subject();

	private selectedTopic: string;

	public ngOnInit(): void {
		this.loadRelationTopics();
	}

	public ngOnDestroy(): void {
		this.destroySubject$.next();
		this.destroySubject$.complete();
		this.treeFacade.ClearTree();
	}

	public loadRelationTopics(): void {
		this.store
			.select(selectCategories)
			.pipe(take(1), takeUntil(this.destroySubject$))
			.subscribe((topics) => {
				this.relationCategoriesItem = topics?.map(
					(topic) => new DropdownList(topic.dcv, topic.name)
				);
			});
	}

	public onRelationshipTypeChanged(topicId: string): void {
		this.selectedRelationship = topicId === '-' ? undefined : topicId;
		if (this.selectedRelationship) {
			this.selectedTopic = topicId;
			this.treeFacade.ExpandTo(topicId);
		}
	}

	public onTopicSelected(topic: string): void {
		this.selectedTopic = topic;
	}

	public saveRelation(): void {
		const sourceDcv = this.sourceDcv;
		const selectedRelationship = this.selectedRelationship;
		this.store
			.select(selectTopicById(this.selectedTopic))
			.pipe(take(1))
			.subscribe((topic) => {
				this.store
					.select(selectTopicById(topic.parent))
					.pipe(take(1))
					.subscribe((parent) => {
						const relation = {
							dcv: selectedRelationship,
							category: this.relationCategoriesItem.find(
								(r) => r.ID === selectedRelationship
							).Name,
							categoryType: this.relationCategoriesItem.find(
								(r) => r.ID === selectedRelationship
							).Name,
							icon: '',
							withElement: this.getTopicRelation(topic),
							withElementParent: this.getTopicRelation(parent),
							status: EditStatus.Created,
							topicDCV: sourceDcv
						} as EditRelation;
						this.relationCreated.emit(relation);
					});
			});
	}

	public cancel(): void {
		this.cancelComponent.emit(false);
	}

	private getTopicRelation(topic: Topic): RelationTopic {
		const { dcv, name, icon } = topic;
		return {
			dcv,
			name,
			icon
		} as RelationTopic;
	}
}
