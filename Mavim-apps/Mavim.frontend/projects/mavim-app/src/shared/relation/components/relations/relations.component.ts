import {
	Component,
	Input,
	Output,
	EventEmitter,
	ChangeDetectionStrategy
} from '@angular/core';
import { Relation } from '../../models/relation.model';

@Component({
	selector: 'mav-relations',
	templateUrl: './relations.component.html',
	styleUrls: ['./relations.component.scss'],
	changeDetection: ChangeDetectionStrategy.OnPush
})
export class RelationsComponent {
	@Output() public internalDcvId = new EventEmitter<string>();
	@Input() public relations: Relation[];

	public emitInternalLinkClickEvent(dcvId: string): void {
		this.internalDcvId.emit(dcvId);
	}

	public relationContainsInstructions(relation: Relation): boolean {
		return (
			!!relation.userInstruction ||
			(relation.dispatchInstructions &&
				relation.dispatchInstructions.length > 0)
		);
	}
}
