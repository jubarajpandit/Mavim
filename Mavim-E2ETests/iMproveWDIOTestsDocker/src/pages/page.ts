/**
 * main page object containing all methods, selectors and functionality
 * that is shared across all page objects
 */
import { utility } from "../Utilities/Utility";

class Page {
  public openImprove(): void {
    const url =
      process.env.PRODUCTION === "true"
        ? process.env.BASE_URL
        : utility.ReadJSON("baseURL");

    browser.url(url);
    browser.maximizeWindow();
  }

  public closeBrowser(): void {
    browser.reloadSession();
  }
}
export const page = new Page();
