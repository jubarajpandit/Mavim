import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { environment } from '../../../environments/environment';
import { MsalService } from '@azure/msal-angular';
import { RoutingFacade } from '../../router/service/routing.facade';
import { LayoutFacade } from '../../layout/service/layout.facade';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AuthorizationFacade } from '../../authorization/service/authorization.facade';
import { ListItem } from '../menu-list/models/list-item.model';
import { LanguageFacade } from '../../language/service/language.facade';
import { Language } from '../../language/enums/language.enum';
import { Role } from '../../authorization/enums/role';

@Component({
	selector: 'mav-o365navbar',
	templateUrl: './o365navbar.component.html',
	styleUrls: ['./o365navbar.component.scss']
})
export class O365navbarComponent implements OnInit, OnDestroy {
	public constructor(
		private readonly authService: MsalService,
		private readonly routingFacade: RoutingFacade,
		private readonly layoutFacade: LayoutFacade,
		private readonly authorizationFacade: AuthorizationFacade,
		private readonly languageFacade: LanguageFacade
	) {
		this.expandedO365Menu = false;
		this.settingsDropdown = false;
		this.accountDropDown = false;
		this.o365Class = 'o365';
	}

	public accountDropDown: boolean;

	@Input() public appsUrl = 'https://portal.office.com';
	public expandedO365Menu: boolean;
	@Input() public languages = 'Languages';
	@Input() public logOut = 'Log out';
	@Input() public myAccount = 'My Account';
	@Input() public navbarTitle = 'o365 title';
	public o365Class: string;
	public settingsDropdown: boolean;
	public userName: string;
	@Input() public userAccountUrl: 'https://portal.office.com/account/';
	public version: string = environment.VERSION;
	public menuListItems: ListItem[] = [];

	private readonly userManagement = 'User management';
	private readonly languageDutch = 'Nederlands';
	private readonly languageEnglish = 'English';
	private readonly destroySubscription = new Subject();

	public ngOnInit(): void {
		if (this.authService.getAccount()) {
			this.userName = this.authService.getAccount().name;
		}
		this.subscribeToSettingsMenu();
		this.subscribeToAuthorization();
	}

	public ngOnDestroy(): void {
		this.destroySubscription.next();
		this.destroySubscription.complete();
	}

	public barBlur(): void {
		// TODO: Do we need to log here? Or what do we do? (WI14764)
	}

	public closeO365Menu(): void {
		this.expandedO365Menu = false;
	}

	public logout(): void {
		this.authService.logout();
	}

	public navigateToO365(): void {
		// TODO: Do we need to log here? Or what do we do? (WI14764)
	}

	public toggleAccountO365Menu(): void {
		this.accountDropDown = !this.accountDropDown;
	}

	public toggleO365Menu(): void {
		this.expandedO365Menu = !this.expandedO365Menu;
	}

	public toggleSettings(): void {
		this.layoutFacade.toggleSettingsMenu();
	}

	public handleMenuItemClicked(name: string): void {
		this.layoutFacade.toggleSettingsMenu();

		if (name === this.userManagement) {
			this.redirectToUserSettings();
		} else if (name === this.languageEnglish) {
			this.setDataLanguage(Language.English);
		} else if (name === this.languageDutch) {
			this.setDataLanguage(Language.Dutch);
		}
	}

	private redirectToUserSettings(): void {
		this.routingFacade.editUsers();
	}

	private setDataLanguage(language: Language): void {
		this.languageFacade.updateLanguage(language);
	}

	private subscribeToSettingsMenu(): void {
		this.layoutFacade.settingsMenu
			.pipe(takeUntil(this.destroySubscription))
			.subscribe((settingsMenuVisibility) => {
				this.settingsDropdown = settingsMenuVisibility;
			});
	}

	private subscribeToAuthorization(): void {
		this.authorizationFacade.getAuthorization
			.pipe(takeUntil(this.destroySubscription))
			.subscribe((auth) => {
				this.menuListItems = [
					{ name: 'Language settings', isHeader: true },
					{ name: this.languageEnglish, isHeader: false },
					{ name: this.languageDutch, isHeader: false }
				];
				if (auth?.role === Role.Administrator) {
					this.menuListItems.push(
						{ name: 'Admin settings', isHeader: true },
						{ name: this.userManagement, isHeader: false }
					);
				}
			});
	}
}
