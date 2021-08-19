import { Component, OnInit, Renderer2 } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { DatabaseFacade } from '../../../database/service/database.facade';
import { MsalService } from '@azure/msal-angular';
import { WopibaseDirective } from '../../shared/wopibase.component';
import { WopiFileType } from '../../enums/wopi-file-types.enums';
import { LanguageFacade } from '../../../language/service/language.facade';

@Component({
	selector: 'mav-word-viewer',
	templateUrl: './word-viewer.component.html',
	styleUrls: ['./word-viewer.component.scss']
})
export class WordViewerComponent extends WopibaseDirective implements OnInit {
	public constructor(
		authService: MsalService,
		renderer: Renderer2,
		sanitizer: DomSanitizer,
		databaseFacade: DatabaseFacade,
		languageFacade: LanguageFacade
	) {
		super(authService, renderer, sanitizer, databaseFacade, languageFacade);
	}

	protected fileType = WopiFileType.Description;
	protected getTitle = 'Word Viewer Frame';
	protected iFrameHeight = '25rem';

	public ngOnInit(): void {
		this.setAuthTokenCallBack(() => this.renderWopiFrame());
	}
}
