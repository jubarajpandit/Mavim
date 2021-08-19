import { editPage } from "../pages/editPage";

class RightPane {
  private readonly waitTime = 1000;
  /** Web element declaration*/
  private get rightPaneEle(): WebdriverIO.Element {
    return $('//router-outlet[@name="right"]/..');
  }

  private get rightPane(): WebdriverIO.Element {
    return $('//div[contains(@class,"screen-2")]');
  }

  private get btnEdit(): WebdriverIO.Element {
    return $(
      '//router-outlet[@name="right"]/..//div[@class="dynamic-panel__edit-btn"]'
    );
  }

  private get titleHeader(): WebdriverIO.Element {
    return $(
      '//router-outlet[@name="right"]/..//h1[@class="panel-title__header"]'
    );
  }

  private get editDescription(): WebdriverIO.Element {
    return this.rightPaneEle.$('i[class="wopi-word-logo"]');
  }

  private get btnWhereAmI(): WebdriverIO.Element {
    return $(
      '//router-outlet[@name="right"]/..//div[@class="dynamic-panel__whereami-btn"]'
    );
  }

  private get relationshipTable(): WebdriverIO.Element {
    return this.rightPaneEle.$('table[class="table relations"]');
  }

  private get fieldsTable(): WebdriverIO.Element {
    return this.rightPaneEle.$(
      '//div[@class="dynamic-panel__content--fields"]'
    );
  }

  private get subTopics(): WebdriverIO.Element {
    return this.rightPaneEle.$(
      '//div[@class="dynamic-panel__content--child-topics"]'
    );
  }

  private get btnFullScreen(): WebdriverIO.Element {
    return $(
      '//div[contains(@class,"screen-2")]//div[@class="split-screens__screen-size-btn"]'
    );
  }

  private get rightPaneFullScreen(): WebdriverIO.Element {
    return $(
      '//div[@class="split-screens__screen screen-2 sidebar_offset__fullscreen rightscreen"]'
    );
  }

  private get collapseRightFullScreen(): WebdriverIO.Element {
    return $(
      '//div[@class="split-screens__screen screen-2 sidebar_offset__fullscreen rightscreen"]//div[@class="split-screens__screen-size-btn"]'
    );
  }

  private get chartArea(): WebdriverIO.Element {
    return this.rightPaneEle.$("//mav-chart-viewer-with-buttons");
  }

  public clickByChartName(chartName: string): void {
    this.chartArea.$('//button[contains(text(), "' + chartName + '")]').click();
  }

  public editTopic(): typeof editPage {
    this.btnEdit.click();
    return editPage;
  }

  public makeRightPaneFullScreen(): void {
    this.btnFullScreen.click();
  }

  public getTopicName(): string {
    return this.titleHeader.getText();
  }

  public getTopicHeader(): string {
    this.titleHeader.scrollIntoView();
    return this.titleHeader.getText();
  }

  public clickEditButton(): void {
    this.editDescription.click();
  }

  public verifyRelationship(
    relationshipType: string,
    relationshipTopic: string
  ): boolean {
    this.relationshipTable.scrollIntoView();
    const relationshipRows = this.relationshipTable.$$(
      '//tr[@class="striped-row"]'
    );
    for (const row of relationshipRows) {
      if (
        row.getText().includes(relationshipTopic) &&
        row.getText().includes(relationshipType)
      ) {
        return true;
      }
    }
    return false;
  }

  public verifyFieldValue(fieldName: string, statusText: string): boolean {
    return this.getCurrentField(fieldName).getText().includes(statusText);
  }

  public verifyMultiLineField(fieldName: string, statusText: string): boolean {
    return this.getCurrentField(fieldName)
      .getText()
      .replace(/\n/g, ",")
      .includes(statusText);
  }

  public waitForRightPaneToLoad(): void {
    this.btnEdit.scrollIntoView();
    this.btnEdit.waitForClickable({ timeout: 20000 });
  }

  public clickOnSubTopic(topicName: string): void {
    const subtopic = this.subTopics.$(
      '//span[@class="subtopics-name" and text()="' + topicName + '"]'
    );
    subtopic.scrollIntoView();
    browser.pause(this.waitTime);
    subtopic.click();
  }

  public checkrightPaneVisible(): boolean {
    return $(
      '//router-outlet[@name="right"]/..//div[contains(@class, "word-edit-btn")]'
    ).isDisplayedInViewport();
  }

  public disableFullScreen(): void {
    this.collapseRightFullScreen.click();
  }

  public clickWhereAmI(): void {
    this.btnWhereAmI.click();
  }

  private getCurrentField(fieldName: string): WebdriverIO.Element {
    return this.fieldsTable.$(
      '//td[contains(@class, "fieldname")]/span[text()="' +
        fieldName +
        '"]/../../td[contains(@class, "fieldvalue")]'
    );
  }

  public clickOnFieldsLink(topicLink: string) {
    this.fieldsTable
      .$(
        '//table[@class="table field"]//span[@class="link" and text()="' +
          topicLink +
          '"]'
      )
      .click();
  }

  public clickOnRelationshipLink(topicLink: string) {
    this.relationshipTable
      .$(
        '//table[@class="table relation"]//span[@class="internal-link" and contains(text(),"' +
          topicLink +
          '")]'
      )
      .click();
  }
}
export const rightPane = new RightPane();
