import { treePane } from "../pages/treePane";
import { utility } from "../Utilities/Utility";

class APIClass {
  public getRequestForBranch(branchName: string): void {
    const defaultWaitingTime = 10000;
    const ResponseCodeOK = 200;
    browser.setTimeout({ pageLoad: 20000 });
    browser.setupInterceptor();
    treePane.expandBranch(branchName);
    browser.pause(defaultWaitingTime);
    const regEx = new RegExp(utility.ReadJSON(branchName));
    browser.expectRequest("GET", regEx, ResponseCodeOK);
    browser.assertRequests();
  }
}
export const api = new APIClass();
