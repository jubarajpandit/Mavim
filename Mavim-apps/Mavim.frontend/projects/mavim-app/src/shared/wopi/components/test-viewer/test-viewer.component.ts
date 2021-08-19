import { Component, OnInit, Renderer2 } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { MsalService } from '@azure/msal-angular';
import { DatabaseFacade } from '../../../database/service/database.facade';
import { LanguageFacade } from '../../../language/service/language.facade';
import { WopiFileType } from '../../enums/wopi-file-types.enums';
import { WopibaseDirective } from '../../shared/wopibase.component';

@Component({
	selector: 'mav-test-viewer',
	templateUrl: './test-viewer.component.html',
	styleUrls: ['./test-viewer.component.scss']
})
export class WopiTestViewerComponent
	extends WopibaseDirective
	implements OnInit
{
	public constructor(
		authService: MsalService,
		renderer: Renderer2,
		sanitizer: DomSanitizer,
		databaseFacade: DatabaseFacade,
		languageFacade: LanguageFacade
	) {
		super(authService, renderer, sanitizer, databaseFacade, languageFacade);
		// TODO: authTokenTtl : represented as the number of milliseconds since January 1, 1970 UTC (the date epoch in JavaScript)
		// For now a value is calculate to be 20 mins further away from current time as this is a requirement from WOPI
		// https://wopi.readthedocs.io/projects/wopirest/en/latest/concepts.html#term-access-token-ttl
		// But we need to see how we do it later.
		const tokenExpirationSeconds = 1200;
		this.authTokenTtl = new Date().setUTCSeconds(tokenExpirationSeconds);
	}
	protected fileType = WopiFileType.Description;
	protected readonly iFrameHeight = '100vh';
	public authTokenTtl = 0;
	protected getTitle = 'Wopi Unit Test';

	public ngOnInit(): void {
		this.setAuthTokenCallBack(() => this.renderWopiFrame());
	}
}
