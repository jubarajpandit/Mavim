import { utility } from "../../Utilities/Utility";
class Wordpage {
  /** web element delaration */
  private get loginBtn(): WebdriverIO.Element {
    return $('//a[@id="b_signInOrgId"]');
  }
  private get descriptionPane(): WebdriverIO.Element {
    return $('//iframe[contains(@title, "Word Editor Frame")]');
  }
  private get description(): WebdriverIO.Element {
    return $('//div[@class="Outline"]');
  }

  public clickLoginButton(): void {
    this.loginBtn.click();
  }

  /**
   * addDescriptionText
   * Adds desctiption text to the description pane
   * string : textDescription void
   */
  public addDescriptionText(textDescription: string): void {
    const docSaveWait = 5000;
    this.waitForDocumentToLoad();
    this.description.click({ x: 0, y: 0 });
    browser.keys("\uE00E");
    browser.keys("\uE00E" + textDescription + "\uE00E");
    browser.keys("\uE007");
    browser.pause(docSaveWait);
    browser.keys(["Control", "s"]);
    browser.pause(docSaveWait);
    this.closeWordTab();
  }

  /**
   * closeWordTab
   * Closes word tab
   * void
   */
  public closeWordTab(): void {
    browser.switchToParentFrame();
    browser.closeWindow();
    utility.switchToFirstTab();
  }

  /**
     *  waitForDocumentToLoad
        waits for the document object to load
     */
  public waitForDocumentToLoad(): void {
    this.descriptionPane.waitForClickable({ timeout: 30000 });
    browser.switchToFrame(this.descriptionPane);
    this.description.waitForClickable({ timeout: 30000 });
  }

  /**
   * getDescriptionText
   * Returns text of desscription
   * string
   */
  public getDescriptionText(): string {
    this.waitForDocumentToLoad();
    this.description.click({ x: 0, y: 0 });
    browser.keys("\uE00E");
    browser.keys("\uE00E");
    const wordText = this.description.getText();
    this.closeWordTab();
    return wordText;
  }
}
export const wordPage = new Wordpage();
