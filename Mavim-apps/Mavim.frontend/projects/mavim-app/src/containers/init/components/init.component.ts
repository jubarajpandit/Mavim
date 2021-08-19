import { Component, OnInit, OnDestroy } from '@angular/core';
import { DatabaseFacade } from '../../../shared/database/service/database.facade';
import { Router } from '@angular/router';
import { filter, takeUntil, take } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { LanguageFacade } from '../../../shared/language/service/language.facade';

@Component({
	selector: 'mav-init',
	templateUrl: 'init.component.html'
})
export class InitComponent implements OnInit, OnDestroy {
	public constructor(
		private readonly databaseFacade: DatabaseFacade,
		private readonly languageFacade: LanguageFacade,
		private readonly router: Router
	) {}

	private readonly initubscription = new Subject();

	public ngOnInit(): void {
		this.getSelectedDatabase();
	}

	public ngOnDestroy(): void {
		this.initubscription.next();
		this.initubscription.complete();
	}

	private getSelectedDatabase(): void {
		this.databaseFacade.selectedDatabase
			.pipe(
				filter((dbid) => !!dbid),
				takeUntil(this.initubscription)
			)
			.subscribe(() => {
				this.languageFacade.language
					.pipe(take(1))
					.subscribe((language) => {
						this.router.navigate([language, 'nav']);
					});
			});
	}
}
