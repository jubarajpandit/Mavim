import { page } from "./page";

/**
 * sub page containing specific selectors and methods for a specific page
 */
class ImproveLogin {
  /**
   * define selectors using getter methods
   */
  private get inputUsername(): WebdriverIO.Element {
    return $('input[name="loginfmt"]');
  }
  private get inputPassword(): WebdriverIO.Element {
    return $('input[name="passwd"]');
  }
  private get btnSubmit(): WebdriverIO.Element {
    return $('input[type="submit"]');
  }
  private get btnYes(): WebdriverIO.Element {
    return $('input[id="idSIButton9"]');
  }

  /**
   * a method to encapsule automation code to interact with the page
   * e.g. to login using username and password
   */
  public login(username: string, password: string): void {
    this.inputUsername.waitForEnabled({ timeout: 30000 });
    this.inputUsername.click();
    this.inputUsername.setValue(username);
    this.btnSubmit.waitForClickable({ timeout: 10000 });
    this.btnSubmit.click();
    this.inputPassword.waitForClickable({ timeout: 10000 });
    this.inputPassword.click();
    this.inputPassword.setValue(password);
    this.btnSubmit.click();
    this.btnYes.click();
  }

  public open(): boolean {
    page.openImprove();
    return true;
  }

  public close(): boolean {
    page.closeBrowser();
    return true;
  }
}

export const improveLogin = new ImproveLogin();
