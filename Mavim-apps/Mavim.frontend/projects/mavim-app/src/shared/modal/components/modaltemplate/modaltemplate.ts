import { Component, EventEmitter, Output } from '@angular/core';

@Component({ template: '' })
export abstract class ModalTemplateComponent {
	@Output() public modelClose = new EventEmitter<boolean>();

	public closeModal(event: boolean): void {
		this.modelClose.emit(event);
	}
}
