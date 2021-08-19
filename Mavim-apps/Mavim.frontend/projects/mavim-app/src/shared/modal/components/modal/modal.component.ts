import { Component, Output, EventEmitter } from '@angular/core';

@Component({
	selector: 'mav-modal',
	templateUrl: './modal.component.html',
	styleUrls: ['./modal.component.scss']
})
export class ModalComponent {
	@Output() public modalVisibility = new EventEmitter<boolean>();

	public removeModal(): void {
		this.modalVisibility.emit(false);
	}
}
