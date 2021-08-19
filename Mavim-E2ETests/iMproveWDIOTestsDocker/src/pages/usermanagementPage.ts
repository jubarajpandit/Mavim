/**
 * main page object containing all methods, selectors and functionality
 * that is shared Usermanagement page objects
 */
import { conformationPopUp } from "../pages/Popups/confirmationPopUp";

class UserManagementPage {
  /** Web element declarations*/
  private get userTable(): WebdriverIO.Element {
    return $('//table[@class="table table-striped"]//tr');
  }
  private get addUserBtn(): WebdriverIO.Element {
    return $('button[class="btn btn-light addUserButton"]');
  }
  private get txtInputUserEmail(): WebdriverIO.Element {
    return $('input[formcontrolname="email"]');
  }
  private get selectRole(): WebdriverIO.Element {
    return $('select[formcontrolname="role"]');
  }
  private get btnAddUserToSystem(): WebdriverIO.Element {
    return $('//mav-add-user//button[@class="btn btn-light addUserButton"]');
  }
  private get messageSuccessAlert(): WebdriverIO.Element {
    return $('//div[@class="alert alert-success"]');
  }

  public verifyUserManagementElements(): boolean {
    this.waitTillUserManagementPageLoads();
    return this.addUserBtn.isExisting();
  }

  public waitTillUserManagementPageLoads(): void {
    const waitTillusertableLoads = 1000;
    browser.pause(waitTillusertableLoads);
    this.userTable.waitForClickable({ timeout: 20000 });
  }

  public verifyUserExists(username: string): boolean {
    this.waitTillUserManagementPageLoads();
    const currentUser = $('//td[contains(text(), "' + username + '")]');
    return currentUser.isExisting();
  }

  public waitTillSuccessMessage(): void {
    this.messageSuccessAlert.waitForExist({ timeout: 10000 });
  }

  public addUser(emailID: string, role: string): void {
    this.waitTillUserManagementPageLoads();
    this.addUserBtn.click();
    this.txtInputUserEmail.setValue(emailID);
    this.selectRole.click();
    this.selectRole.selectByVisibleText(role);
    this.txtInputUserEmail.click();
    browser.keys("Enter");
    this.btnAddUserToSystem.click();
    this.waitTillUserManagementPageLoads();
    this.waitTillSuccessMessage();
  }

  public clickAddUserButton(): void {
    this.waitTillUserManagementPageLoads();
    this.addUserBtn.click();
  }

  public removeUser(emailID: string): void {
    if (this.verifyUserExists(emailID)) {
      const currentRow = this.getUserRow(emailID);
      currentRow.$('div[class="user-actions-image delete-icon"]').click();
      conformationPopUp.clickConfirm();
      this.waitTillUserManagementPageLoads();
      this.waitTillSuccessMessage();
    }
  }

  public getUserRow(emailID: string): WebdriverIO.Element {
    return $('//td[contains(text(),"' + emailID + '")]/..');
  }
}
export const usernamagementPage = new UserManagementPage();
