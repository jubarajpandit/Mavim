import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { distinctUntilChanged } from 'rxjs/operators';
import { FormValidationError } from '../../../containers/edit/models/formvalidationerror.model';
import {
	maximalLength,
	minimalLength
} from '../../constants/topic-name-validation.constants';

@Component({
	selector: 'mav-topic-edit-name',
	templateUrl: './topic-edit-name.component.html',
	styleUrls: ['./topic-edit-name.component.scss']
})
export class TopicEditNameComponent implements OnInit {
	@Input() public titleText = '';
	@Output() public topicNameEditError =
		new EventEmitter<FormValidationError>();
	@Output() public nameChanged = new EventEmitter();

	public readonly minLength = minimalLength;
	public readonly maxLength = maximalLength;
	public readonly componentName = 'TopicEditNameComponent';
	public editFieldName = 'title';
	public titleTextFormControl: FormControl;

	public ngOnInit(): void {
		this.titleTextFormControl = new FormControl(this.titleText, [
			Validators.minLength(minimalLength),
			Validators.maxLength(maximalLength)
		]);
		this.titleTextFormControl.valueChanges
			.pipe(distinctUntilChanged())
			.subscribe((value) => {
				this.onNameChangedEvent(value);
			});
	}

	public onNameChangedEvent(value: string): void {
		if (this.titleTextFormControl.valid) {
			this.nameChanged.emit(value);
		} else {
			const formValidationError: FormValidationError =
				new FormValidationError();
			formValidationError.componentName = this.componentName;
			formValidationError.errorType = Object.keys(
				this.titleTextFormControl.errors
			).join(',');
			this.topicNameEditError.emit(formValidationError);
		}
	}
}
