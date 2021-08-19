import {
	Component,
	OnChanges,
	Renderer2,
	SecurityContext,
	SimpleChanges
} from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { DatabaseFacade } from '../../../database/service/database.facade';
import { MsalService } from '@azure/msal-angular';
import { WopibaseDirective } from '../../shared/wopibase.component';
import { WopiFileType } from '../../enums/wopi-file-types.enums';
import { LanguageFacade } from '../../../language/service/language.facade';
import { FeatureflagFacade } from '../../../featureflag/service/featureflag.facade';
import { TopicFacade } from '../../../topic/services/topic.facade';
import { NotificationService } from '../../../notification/services/notification.service';
import { combineLatest, Observable, Subject } from 'rxjs';
import { filter, take, takeUntil, tap } from 'rxjs/operators';
import { NotificationTypes } from '../../../notification/enums/notification-types.enum';
import { WopiFeatures } from '../../enums/wopi-feature-flags.enum';
import { Language } from '../../../language/enums/language.enum';
import { Topic } from '../../../topic/models/topic.model';

@Component({
	selector: 'mav-chart-viewer',
	templateUrl: './chart-viewer.component.html',
	styleUrls: ['./chart-viewer.component.scss']
})
export class ChartViewerComponent
	extends WopibaseDirective
	implements OnChanges
{
	public constructor(
		authService: MsalService,
		renderer: Renderer2,
		sanitizer: DomSanitizer,
		databaseFacade: DatabaseFacade,
		languageFacade: LanguageFacade,
		private readonly topicFacade: TopicFacade,
		private readonly notificationService: NotificationService,
		private readonly featureFacade: FeatureflagFacade
	) {
		super(authService, renderer, sanitizer, databaseFacade, languageFacade);
	}

	protected readonly fileType = WopiFileType.Chart;
	protected readonly iFrameHeight = '40.625rem';
	protected getTitle = 'Chart Viewer Frame';
	private readonly httpSuccessCode = 200;
	private session: OfficeExtension.EmbeddedSession;

	public ngOnChanges(changes: SimpleChanges): void {
		if (this.dcvId === undefined) {
			return;
		}
		if (!changes[this.dcvIdKey].firstChange) {
			this.removeWopiFrame();
		}
		this.iFrameId = `embed-iframe-${this.dcvId}`;

		this.featureFacade
			.getFeatureflag(WopiFeatures.navigateChart)
			.pipe(take(1))
			.subscribe((featureEnabled) => {
				if (featureEnabled) {
					this.setAuthTokenCallBack(() =>
						this.renderWopiFrameFromSession(
							this.databaseFacade.selectedDatabase,
							this.getAuthToken(),
							this.languageFacade.language
						)
					);
				} else {
					this.setAuthTokenCallBack(() => this.renderWopiFrame());
				}
			});
	}

	private removeWopiFrame(): void {
		if (this.iFrameHolder) {
			this.renderer.removeChild(
				this.iFrameHolder.nativeElement,
				document.getElementById(this.iFrameId)
			);
		}
	}

	// Loads the Visio application and Initializes communication between developer frame and Visio online frame
	private renderWopiFrameFromSession(
		selectedDatabase: Observable<string>,
		authToken: Observable<string>,
		languageObservable: Observable<Language>
	): void {
		combineLatest([selectedDatabase, authToken, languageObservable])
			.pipe(
				tap(([dbid, token, language]) => {
					this.authToken = token;
					this.safeUrl = this.enrichUrl(dbid, language);
				}),
				take(1)
			)
			.subscribe(() => {
				const sanitizeUrl = this.sanitizer.sanitize(
					SecurityContext.RESOURCE_URL,
					this.safeUrl
				);

				const embeddedOptions = {
					id: this.iFrameId,
					container: this.iFrameHolder.nativeElement as HTMLElement,
					webApplication: {
						accessToken: this.authToken,
						accessTokenTtl: '0'
					},
					height: this.iFrameHeight
				} as OfficeExtension.EmbeddedOptions;

				this.session = new OfficeExtension.EmbeddedSession(
					sanitizeUrl,
					embeddedOptions
				);

				this.session.init().then(() => {
					if (this.fileType === WopiFileType.Chart) {
						this.AttachVisioHandler();
					}
				});
			});
	}

	private AttachVisioHandler(): void {
		Visio.run(
			this.session,
			(context: Visio.RequestContext): Promise<void> => {
				context.document.onSelectionChanged.add(
					async (
						args: Visio.SelectionChangedEventArgs
						// eslint-disable-next-line @typescript-eslint/require-await
					): Promise<void> =>
						this.handleClickShapeEvent(args?.shapeNames[0])
				);
				return context.sync();
			}
		);
	}

	private handleClickShapeEvent(encodedTopicId: string): void {
		if (!encodedTopicId?.endsWith('S') || !encodedTopicId?.startsWith('M'))
			return;

		const topicId = this.decodeEncodedTopicId(encodedTopicId);
		const destroySubscription = new Subject();
		let fetchTopic = false;
		this.topicFacade
			.getTopicByDcv(topicId)
			.pipe(
				tap((topic) => {
					if (!topic?.resources && !fetchTopic) {
						fetchTopic = true;
						this.topicFacade.loadTopicByDcv(topicId);
					}
				}),
				filter((topic) => !!topic?.resources),
				takeUntil(destroySubscription)
			)
			.subscribe((topic) => {
				if (this.isTopicAllowedToNavigate(topic)) {
					this.shapeClick.emit(topicId);
				} else {
					this.notificationService.sendNotification(
						NotificationTypes.Info,
						'Shape Link does not work, please update the chart in the Mavim Manager'
					);
				}

				destroySubscription.next();
				destroySubscription.complete();
			});
	}

	private decodeEncodedTopicId(encodedTopicId: string): string {
		let topicId = '';
		const startIndexDatabase = 1;
		const startIndexCode = 9;
		const startIndexVersion = 17;
		const endIndex = 25;

		const database =
			'0x' + encodedTopicId.substring(startIndexDatabase, startIndexCode);
		const code =
			'0x' + encodedTopicId.substring(startIndexCode, startIndexVersion);
		const version =
			'0x' + encodedTopicId.substring(startIndexVersion, endIndex);
		topicId += `d${parseInt(String(Number(database)), 10)}`;
		topicId += `c${parseInt(String(Number(code)), 10)}`;
		topicId += `v${parseInt(String(Number(version)), 10)}`;

		return topicId;
	}

	private isTopicAllowedToNavigate(topic: Topic): boolean {
		return (
			topic.httpStatusCode === this.httpSuccessCode &&
			!topic.isInRecycleBin
		);
	}
}
