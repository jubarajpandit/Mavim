import { Component, Input, Output, EventEmitter } from '@angular/core';
import { EditStatus } from '../../../../containers/edit/enums/edit-status.enum';
import { EditRelation } from '../../models/edit-relation.model';

@Component({
	selector: 'mav-relations-edit',
	templateUrl: './relations-edit.component.html',
	styleUrls: ['./relations-edit.component.scss']
})
export class RelationsEditComponent {
	@Input() public relations: EditRelation[] = [];
	@Input() public relationFeatureFlag = false;
	@Output() public addRelationEvent: EventEmitter<EditRelation> =
		new EventEmitter<EditRelation>();
	@Output() public deleteRelationEvent: EventEmitter<EditRelation> =
		new EventEmitter<EditRelation>();
	@Output() public resetRelationEvent: EventEmitter<EditRelation> =
		new EventEmitter<EditRelation>();

	public relationContainsInstructions(relation: EditRelation): boolean {
		return (
			!!relation.userInstruction ||
			(!!relation.dispatchInstructions &&
				relation.dispatchInstructions.length > 0)
		);
	}

	public deleteRelation(relation: EditRelation): void {
		this.deleteRelationEvent.emit(relation);
	}

	public addRelation(): void {
		this.addRelationEvent.emit(new EditRelation());
	}

	public resetRelation(relation: EditRelation): void {
		this.resetRelationEvent.emit(relation);
	}

	public isCreatedRelation(relation: EditRelation): boolean {
		return relation.status === EditStatus.Created;
	}

	public isDeleted(relation: EditRelation): boolean {
		return relation.status === EditStatus.Deleted;
	}
}
