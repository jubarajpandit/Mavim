import { Pipe, PipeTransform } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Pipe({
	name: 'secure'
})
export class SecurePipe implements PipeTransform {
	constructor(
		protected http: HttpClient,
		protected sanitizer: DomSanitizer
	) {}

	transform(url: string): Observable<SafeUrl> {
		return this.http.get(url, { responseType: 'blob' }).pipe(
			map((val) => {
				return this.sanitizer.bypassSecurityTrustUrl(
					URL.createObjectURL(val)
				);
			})
		);
	}
}
