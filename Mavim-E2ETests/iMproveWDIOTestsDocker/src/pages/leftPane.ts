class LeftPane {
  private readonly waitTime = 1000;
  private get leftPane(): WebdriverIO.Element {
    return $('//router-outlet[@name="left"]/..');
  }
  private get subTopics(): WebdriverIO.Element {
    return this.leftPane.$(
      '//div[@class="dynamic-panel__content--child-topics"]'
    );
  }
  private get leftpaneTitle(): WebdriverIO.Element {
    return this.leftPane.$("//h1");
  }
  private get btnFullScreen(): WebdriverIO.Element {
    return $('//div[@class="split-screens__screen-size-btn"]');
  }

  private get verticalSideBar(): WebdriverIO.Element {
    return $('h3[class="sidebar-vertical-title"]');
  }

  private get leftPaneFullScreen(): WebdriverIO.Element {
    return $(
      '//div[@class="split-screens__screen screen-1 sidebar_offset__fullscreen"]'
    );
  }

  private get collapseLeftFullScreen(): WebdriverIO.Element {
    return this.leftPaneFullScreen.$(
      '//div[@class="split-screens__screen-size-btn"]'
    );
  }

  private get btnEdit(): WebdriverIO.Element {
    return this.leftPane.$('//div[@class="dynamic-panel__edit-btn"]');
  }

  private get btnWhereAmI(): WebdriverIO.Element {
    return $(
      '//router-outlet[@name="left"]/../..//div[@class="dynamic-panel__whereami-btn"]'
    );
  }

  public ClickOnSubtopic(topicName: string): void {
    const subtopic = this.subTopics.$(
      '//span[@class="subtopics-name" and text()="' + topicName + '"]'
    );
    browser.pause(this.waitTime);
    subtopic.scrollIntoView();
    subtopic.click();
  }

  public clickOnverticalSidebar(): void {
    this.verticalSideBar.click();
  }

  public getLeftPaneTitle(): string {
    return this.leftpaneTitle.getText();
  }

  public clickFullScreen(): void {
    this.btnFullScreen.click();
  }

  public disableFullScreen(): void {
    this.collapseLeftFullScreen.click();
  }

  public checkLeftPaneIsVisible(): boolean {
    return $('//router-outlet[@name="left"]/..//div[contains(@class, "word-edit-btn")]').isDisplayedInViewport();
  }

  public getVerticalBarText(): string {
    return this.verticalSideBar.getText();
  }

  public waitForLeftPaneToLoad(): void {
    this.btnEdit.waitForClickable({ timeout: 20000 });
  }

  public clickWhereAmI(): void {
    this.btnWhereAmI.click();
  }
}
export const leftPane = new LeftPane();
