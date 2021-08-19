import { Component, Input, forwardRef, OnInit, OnDestroy } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Dictionary } from '@ngrx/entity';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { SelectDropDownItem } from './models/select-dropdown-item.model';
import { SelectDropdownModel } from './models/select-dropdown.model';

@Component({
	selector: 'mav-select-dropdown',
	templateUrl: './select-dropdown.component.html',
	styleUrls: ['./select-dropdown.component.scss'],
	providers: [
		{
			provide: NG_VALUE_ACCESSOR,
			useExisting: forwardRef(() => SelectDropdownComponent),
			multi: true
		}
	]
})
export class SelectDropdownComponent
	implements OnInit, OnDestroy, ControlValueAccessor
{
	@Input() public disabled = false;
	@Input() public values: Dictionary<SelectDropdownModel> = undefined;
	@Input() public selectedItem: SelectDropDownItem = undefined;
	@Input() public placeholder = 'Please select a value';

	public filteredValues: Dictionary<SelectDropdownModel>;
	public dropdownVisible = false;

	public get selectedIcon(): string {
		return this.selectedItem?.value?.resourceId ?? '';
	}

	public get selectedName(): string {
		return this.selectedItem?.value?.name ?? '';
	}

	public filter$ = new Subject<string>();

	private onChanged: (_: string) => void;
	private onTouched: (_: string) => void;
	private readonly debounceTime = 400;

	public ngOnInit(): void {
		this.filteredValues = this.values;
		this.subscribeToFilter();
	}

	public ngOnDestroy(): void {
		this.filter$.unsubscribe();
	}

	public toggleDropdown(): void {
		if (this.disabled) return;
		this.dropdownVisible = !this.dropdownVisible;
	}

	public writeValue(item: SelectDropDownItem): void {
		if (!(item?.key in this.values)) return;

		this.filter$.next('');
		this.selectedItem = item;
		this.onChanged(item.key);
		this.onTouched(item.key);
		this.dropdownVisible = false;
	}

	public clearValue(): void {
		this.selectedItem = undefined;
		this.onChanged('');
		this.onTouched('');
		this.dropdownVisible = false;
	}

	public registerOnChange(fn: (_: string) => void): void {
		this.onChanged = fn;
	}

	public registerOnTouched(fn: (_: string) => void): void {
		this.onTouched = fn;
	}

	public setDisabledState?(isDisabled: boolean): void {
		this.disabled = isDisabled;
	}

	public notEmpty(dict: Dictionary<SelectDropdownModel>): boolean {
		return Object.keys(dict).length > 0;
	}

	private subscribeToFilter(): void {
		this.filter$
			.pipe(
				tap(() => (this.dropdownVisible = true)),
				debounceTime(this.debounceTime),
				distinctUntilChanged()
			)
			.subscribe((filter) => {
				if (!filter) {
					this.filteredValues = this.values;
				} else {
					this.filteredValues = Object.fromEntries(
						Object.entries(this.values).filter(([, v]) =>
							v?.name.toLowerCase().includes(filter.toLowerCase())
						)
					);
				}
			});
	}
}
