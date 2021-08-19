import { Directive, OnInit } from '@angular/core';
import { SafeUrl, DomSanitizer } from '@angular/platform-browser';
import { DatabaseFacade } from '../../database/service/database.facade';
import { LanguageFacade } from '../../language/service/language.facade';
import { combineLatest } from 'rxjs';
import { tap, take } from 'rxjs/operators';
import { Language as LanguageType } from '../../language/enums/language.enum';
import { DatabaseID } from '../../database/constants';
import { Language } from '../../language/constants';
import { TopicPathPrefix } from '../../../environments/constants';
import { UrlUtils } from '../../utils';

@Directive()
export abstract class CustomIconBase implements OnInit {
	constructor(
		protected readonly sanitizer: DomSanitizer,
		protected readonly databaseFacade: DatabaseFacade,
		protected readonly languageFacade: LanguageFacade
	) {}

	private readonly topicCustomIconsApiUrl = `${TopicPathPrefix}/topic/{customIconId}/customicon`;

	public imageUrl: string;
	public blobUrl: SafeUrl;
	public customIconId: string;

	public ngOnInit(): void {
		this.setSafeUrlAsync();
	}

	protected setSafeUrlAsync(): void {
		combineLatest([
			this.databaseFacade.selectedDatabase,
			this.languageFacade.language
		])
			.pipe(
				tap(([dbid, language]) => {
					this.imageUrl = this.enrichUrl(dbid, language);
				}),
				take(1)
			)
			.subscribe();
	}

	protected enrichUrl(dbid: string, language: LanguageType): string {
		let customIconApiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.topicCustomIconsApiUrl,
			{
				customIconId: this.customIconId
			}
		);

		customIconApiUrl = customIconApiUrl.replace(DatabaseID, dbid);
		customIconApiUrl = customIconApiUrl.replace(Language, language);

		return customIconApiUrl;
	}
}
