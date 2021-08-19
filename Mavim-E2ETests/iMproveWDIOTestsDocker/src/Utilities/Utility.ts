import * as jsonData from "../Data/apiData.json";
import * as sessionStoreage from "../Data//sessionStoreage.json";
import * as devPasswords from "../Data/passwordData.json";
import * as navigateLinks from "../Data/navigateLinks.json";
import { Dictionary } from "../support/lib/Dictionary";

class Utility {
  public readNavigateLinks(key: string): string {
    return (navigateLinks as Dictionary<string>)[key];
  }

  public ReadJSON(key: string): string {
    return (jsonData as Dictionary<string>)[key];
  }

  /**
   *  readUserNameAndPassword
   * Returns USN and password value for different branches
  user name and password string : return string */
  public readUserNameAndPassword(key: string): string {
    const jsonData =
      process.env.PRODUCTION === "true"
        ? {
            userName: process.env.USER_NAME,
            password: process.env.USER_PASS,
            userName2: "testautomationuser2@devmavimcloud.onmicrosoft.com",
          }
        : devPasswords;
    return (jsonData as Dictionary<string>)[key];
  }

  /**
   *  getSessionStoreageValue
   * Returns session storeage value for different branches
  sessionVal string : return string */
  public getSessionStoreageValue(key: string): string {
    return (sessionStoreage as Dictionary<string>)[key];
  }

  /**
   *  verifySessionStoreageValue
   * Verify if storeage session "MAVNAV" has the given content
  sessionVal string : return boolean */
  public verifySessionStoreageValue(sessionVal: string): boolean {
    const sessionStoreage = this.getSessionStoreageFullString();
    return sessionStoreage.includes(sessionVal);
  }

  public getSessionStoreageFullString(): string {
    const sessionStoreage = browser.execute(
      "return window.sessionStorage.getItem('MAVNAV')"
    );
    return sessionStoreage as string;
  }

  public getSessionStoreageCount(): number {
    const jsonData = this.getSessionStoreageFullString();
    return jsonData.split(",").length;
  }

  /**
   *  getURL
   * Returns current url value
    return string */
  public getURL(): string {
    return browser.getUrl();
  }

  public back(): void {
    browser.back();
  }

  public forward(): void {
    browser.forward();
  }

  /**
   *  switchToWordTab
   * switches to word window
  : void */
  public switchToWordTab(): void {
    browser.switchToWindow(browser.getWindowHandles()[1]);
    browser.waitUntil(() => browser.getUrl().toLowerCase().includes("word"), {
      timeout: 30000,
    });
  }

  /**
   *  switchToFirstTab
   * switches to first window
  : void */
  public switchToFirstTab(): void {
    browser.switchToWindow(browser.getWindowHandles()[0]);
  }

  /**
   *  getRandomString
   * returns random string of given length
  : return string */
  public getRandomString(stringLength: number): string {
    let text = "";
    const charSet =
      "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    for (let i = 0; i < stringLength; i++) {
      text += charSet.charAt(Math.floor(Math.random() * charSet.length));
    }
    return text;
  }

  public navigateToLinks(linkKey: string): void {
    browser.url(this.getBaseURL() + this.readNavigateLinks(linkKey));
  }

  public switchtoIframe(xpath: string): void {
    browser.switchToFrame($(xpath));
  }

  public switchToDefaultContent(): void {
    browser.switchToParentFrame();
  }

  public getBaseURL(): string {
    const url =
      process.env.PRODUCTION === "true"
        ? process.env.BASE_URL
        : this.ReadJSON("baseURL");
    return url;
  }
}
export const utility = new Utility();
