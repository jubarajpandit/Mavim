/* eslint-disable @typescript-eslint/unbound-method */
import { Component, Input, OnDestroy } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { Validators } from '@angular/forms';
import { Dictionary } from '@ngrx/entity';
import { combineLatest, Subject } from 'rxjs';
import { Observable } from 'rxjs';
import { distinctUntilChanged, take, takeUntil, tap } from 'rxjs/operators';
import { SelectDropDownItem } from '../../../components/select-dropdown/models/select-dropdown-item.model';
import { SelectDropdownModel } from '../../../components/select-dropdown/models/select-dropdown.model';
import {
	maximalLength,
	minimalLength
} from '../../../constants/topic-name-validation.constants';
import { ModalTemplateComponent } from '../../../modal/components/modaltemplate/modaltemplate';
import { TopicMetaType } from '../../../topic-meta/models/topic-meta-type.model';
import { TopicMetaFacade } from '../../../topic-meta/services/topic-meta.facade';

@Component({ template: '' })
export abstract class AddTopicBaseTemplateComponent
	extends ModalTemplateComponent
	implements OnDestroy
{
	protected constructor(public readonly topicMetaFacade: TopicMetaFacade) {
		super();

		this.topicForm = new FormGroup({
			title: new FormControl('', [
				Validators.required,
				Validators.minLength(minimalLength),
				Validators.maxLength(maximalLength)
			]),
			type: new FormControl('', [Validators.required]),
			icon: new FormControl('', [Validators.required])
		});
	}

	public iconDisabledInput: SelectDropDownItem = undefined;
	public typePlaceholder = 'Please select a type';
	public iconPlaceholder = 'Please select an icon';

	public get titleFormControl(): AbstractControl {
		return this.topicForm.get('title');
	}

	public get typeFormControl(): AbstractControl {
		return this.topicForm.get('type');
	}

	public get iconFormControl(): AbstractControl {
		return this.topicForm.get('icon');
	}

	public get topicTypeValueChanges(): Observable<string> {
		return this.topicForm.get('type').valueChanges;
	}

	public get nameFormControlErrorMessage(): string {
		const maxLengthKey = 'maxlength';
		return this.titleFormControl.errors &&
			this.titleFormControl.errors[maxLengthKey]
			? 'Name cannot exceed 2000 characters'
			: 'Please fill in a name to proceed';
	}

	public topicForm: FormGroup;
	public types: Observable<Dictionary<TopicMetaType>>;
	public typesLoaded: Observable<boolean>;
	public icons: Observable<Dictionary<string>>;
	public iconsLoaded: Observable<boolean>;

	@Input() public modalTitle: string;

	protected readonly destroySubscription = new Subject();

	public abstract accept(): void;

	public ngOnDestroy(): void {
		this.topicMetaFacade.clearStore();
		this.destroySubscription.next();
		this.destroySubscription.complete();
	}

	public mapIconsToDropDownModel(
		dict: Dictionary<string>
	): Dictionary<SelectDropdownModel> {
		return Object.fromEntries(
			Object.entries(dict).map(([key, value]) => [
				key,
				{ resourceId: key, name: value } as SelectDropdownModel
			])
		);
	}

	public cancel(): void {
		this.modelClose.emit(false);
	}

	protected subscribeToTypesValueChanges(): void {
		this.topicTypeValueChanges
			.pipe(
				distinctUntilChanged(),
				tap(() => {
					this.iconFormControl.setValue('');
				}),
				takeUntil(this.destroySubscription)
			)
			.subscribe((type) => {
				if (type) {
					this.topicMetaFacade.getTypes
						.pipe(take(1))
						.subscribe((types) => {
							if (types[type].isSystemName) {
								this.setupFormForSystemType(types[type]);
							} else {
								this.setupFormAndLoadIcons(type);
							}
						});
				}
			});
	}

	protected subscribeToTypesAndIcons(type: string): void {
		const destroyLocalSub = new Subject();
		combineLatest([
			this.topicMetaFacade.getTypes,
			this.topicMetaFacade.getIcons,
			this.topicMetaFacade.getIconsLoaded
		])
			.pipe(takeUntil(destroyLocalSub))
			.subscribe(([types, icons, iconsLoaded]) => {
				if (iconsLoaded) {
					destroyLocalSub.next();
					destroyLocalSub.complete();
					if (Object.keys(icons).length <= 1) {
						this.setIconFixed(types[type]);
					} else {
						this.resetIconFormControl();
					}
				}
			});
	}

	private setIconFixed(type: TopicMetaType): void {
		this.iconFormControl.setValue(type.resourceId);
		this.iconFormControl.disable();
		this.iconDisabledInput = {
			key: type.resourceId,
			value: { name: type.name, resourceId: type.resourceId }
		} as SelectDropDownItem;
	}

	private resetIconFormControl(): void {
		this.iconFormControl.enable();
		this.iconDisabledInput = undefined;
	}

	private setupFormForSystemType(type: TopicMetaType): void {
		this.iconFormControl.setValue(type.resourceId);
		this.iconFormControl.disable();
		this.titleFormControl.disable();
		this.titleFormControl.setValue(type.name);
		this.iconDisabledInput = {
			key: type.resourceId,
			value: { name: type.name, resourceId: type.resourceId }
		} as SelectDropDownItem;
	}

	private setupFormAndLoadIcons(type: string): void {
		this.titleFormControl.enable();
		this.topicMetaFacade.loadIcons(type);
		this.subscribeToTypesAndIcons(type);
	}
}
