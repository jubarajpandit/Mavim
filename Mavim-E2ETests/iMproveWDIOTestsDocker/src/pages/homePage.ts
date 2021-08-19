class HomePage {
  /** web element delaration */
  private get btnHomePage(): WebdriverIO.Element {
    return $('i[class="mdl2 mdl2-home"]');
  }
  private get btnProfile(): WebdriverIO.Element {
    return $('div[class="o365__bio-name"]');
  }
  private get linkLogout(): WebdriverIO.Element {
    return $('li[class="logout-link"]');
  }
  private get accountIcon(): WebdriverIO.Element {
    return $('img[class="tile-img"]');
  }
  private get settingsBtn(): WebdriverIO.Element {
    return $(
      'div[class="mdl2 mdl2-settings o365__button o365__button--settings"]'
    );
  }
  private get linkUserManagement(): WebdriverIO.Element {
    return $("=User management");
  }
  private get rootHeader(): WebdriverIO.Element {
    return $('//h1[text()="Mavim Database"]');
  }
  private get notificationView(): WebdriverIO.Element {
    return $('div[class="main-view__notifications"]');
  }
  private get notificationMessage(): WebdriverIO.Element {
    return this.notificationView.$('div[class="notification__message"]');
  }
  private get btnCloseNotification(): WebdriverIO.Element {
    return this.notificationView.$('button[class="btn close"]');
  }

  public clickHomeButton(): void {
    this.btnHomePage.click();
  }

  public logout(): void {
    this.btnProfile.scrollIntoView();
    this.btnProfile.click();
    this.linkLogout.click();
    this.accountIcon.click();
  }

  public waitForHomeButton(): void {
    this.rootHeader.waitForClickable({ timeout: 60000 });
  }

  public navigateToUserManagement(): void {
    this.settingsBtn.scrollIntoView();
    this.settingsBtn.click();
    this.linkUserManagement.click();
  }

  public changeLanguage(language: string): void {
    this.settingsBtn.click();
    $('//a[contains(text(),"' + language + '")]').click();
  }

  public getNotificationMessage(): string {
    this.notificationMessage.scrollIntoView();
    return this.notificationMessage.getText();
  }

  public closeNotificationMessage(): void {
    this.btnCloseNotification.click();
  }
}
export const homePage = new HomePage();
