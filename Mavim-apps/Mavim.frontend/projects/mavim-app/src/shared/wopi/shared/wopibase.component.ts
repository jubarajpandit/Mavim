/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-unsafe-return */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import {
	ViewChild,
	Input,
	ElementRef,
	Renderer2,
	Directive,
	Output,
	EventEmitter
} from '@angular/core';
import { SafeResourceUrl, DomSanitizer } from '@angular/platform-browser';
import { UrlUtils } from '../../utils';
import { MsalService } from '@azure/msal-angular';
import { DatabaseFacade } from '../../database/service/database.facade';
import { from, Observable, combineLatest } from 'rxjs';
import { tap, map, take } from 'rxjs/operators';
import { DatabaseID } from '../../database/constants';
import { environment } from '../../../environments/environment';
import { WopiFileType } from '../enums/wopi-file-types.enums';
import { Language } from '../../language/constants';
import { LanguageFacade } from '../../language/service/language.facade';
import { Language as LanguageType } from '../../language/enums/language.enum';

@Directive()
export abstract class WopibaseDirective {
	public constructor(
		protected readonly authService: MsalService,
		protected readonly renderer: Renderer2,
		protected readonly sanitizer: DomSanitizer,
		protected readonly databaseFacade: DatabaseFacade,
		protected readonly languageFacade: LanguageFacade
	) {}

	@ViewChild('wopi_form') public form: ElementRef;
	@ViewChild('frameholder') public iFrameHolder: ElementRef;
	@Input() public dcvId: string;
	@Input() public wopiActionUrl: string;

	@Output() public shapeClick = new EventEmitter<string>();

	public authToken: string;
	public safeUrl: SafeResourceUrl;
	protected abstract readonly fileType: WopiFileType;
	protected abstract readonly iFrameHeight: string;
	protected abstract get getTitle(): string;
	protected iFrameId: string;
	private iFrame: HTMLIFrameElement;

	protected readonly dcvIdKey = 'dcvId';

	protected getAuthToken(): Observable<string> {
		const scopes = this.authService.getScopesForEndpoint(
			`${environment.wopiSrc}/**`
		);
		return from(this.authService.acquireTokenSilent({ scopes })).pipe(
			map((response) => {
				return response.idToken.rawIdToken;
			})
		);
	}

	protected enrichUrl(dbid: string, language: LanguageType): SafeResourceUrl {
		const wopiClientPath =
			environment.wopiSrc +
			`/v1/${DatabaseID}/${Language}/${this.fileType}/wopi/files/{dcvId}`;
		const wopiClientSrcWithToken = UrlUtils.getUrlByAbsoluteApiUrl(
			wopiClientPath,
			{
				dcvId: this.dcvId
			}
		);
		let unsafeUrl = UrlUtils.getUrlByAbsoluteApiUrl(this.wopiActionUrl, {
			wopiSrc: wopiClientSrcWithToken
		});
		unsafeUrl = unsafeUrl.replace(DatabaseID, dbid);
		unsafeUrl = unsafeUrl.replace(Language, language);

		return this.sanitizer.bypassSecurityTrustResourceUrl(unsafeUrl);
	}

	protected setAuthTokenCallBack(renderWopiFrameCallback: () => void): void {
		this.setAuthTokenAndSafeUrlAsync().subscribe(() => {
			renderWopiFrameCallback();
		});
	}

	protected renderWopiFrame(): void {
		this.iFrame = this.renderer.createElement('iframe');
		this.iFrame.name = this.dcvId;
		this.iFrame.id = this.iFrameId;
		// The title should be set for accessibility
		this.iFrame.title = `${this.getTitle} ${this.dcvId}`;
		// This attribute allows true fullscreen mode in slideshow view
		// when using PowerPoint's 'view' action.
		this.iFrame.setAttribute('allowfullscreen', 'true');
		// The sandbox attribute is needed to allow automatic redirection to the O365 sign-in page in the business user flow
		this.iFrame.setAttribute(
			'sandbox',
			'allow-scripts allow-same-origin allow-forms allow-popups allow-top-navigation allow-popups-to-escape-sandbox'
		);
		this.renderer.appendChild(this.iFrameHolder.nativeElement, this.iFrame);
		setTimeout(() => this.form.nativeElement.submit(), 0);
	}

	private setAuthTokenAndSafeUrlAsync(): Observable<
		[string, string, LanguageType]
	> {
		return combineLatest([
			this.databaseFacade.selectedDatabase,
			this.getAuthToken(),
			this.languageFacade.language
		]).pipe(
			tap(([dbid, token, language]) => {
				this.authToken = token;
				this.safeUrl = this.enrichUrl(dbid, language);
			}),
			take(1)
		);
	}
}
