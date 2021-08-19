import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
	name: 'uppercaseFirstChar'
})
export class UppercaseFirstCharacterPipe implements PipeTransform {
	public transform(value: string): string {
		return value ? value[0]?.toUpperCase() + value?.slice(1) : '';
	}
}
