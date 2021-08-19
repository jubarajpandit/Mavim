import { Then, When } from "cucumber";
import { homePage } from "../pages/homePage";
import { usernamagementPage } from "../pages/usermanagementPage";
import { utility } from "../Utilities/Utility";

When("I navigate to User Management Page", () => {
  homePage.navigateToUserManagement();
  usernamagementPage.waitTillUserManagementPageLoads();
  expect(usernamagementPage.verifyUserManagementElements()).toEqual(true);
});

Then("I should verify user {string} exists", (userNameparam: string) => {
  const usn = utility.readUserNameAndPassword(userNameparam);
  expect(usernamagementPage.verifyUserExists(usn)).toEqual(true);
});

Then(
  "I should be able to change the language to {string}",
  (language: string) => {
    homePage.changeLanguage(language);
  }
);

Then(
  "I should be able to add user {string} with role as {string}",
  (userID: string, role: string) => {
    const usn = utility.readUserNameAndPassword(userID);
    usernamagementPage.addUser(usn, role);
  }
);

Then("I should be able to remove user {string}", (userID: string) => {
  const usn = utility.readUserNameAndPassword(userID);
  usernamagementPage.removeUser(usn);
});

Then("I should verify user {string} has been removed", (userID: string) => {
  const usn = utility.readUserNameAndPassword(userID);
  expect(usernamagementPage.verifyUserExists(usn)).toEqual(false);
});

Then("I wait for {int} seconds", (seconds: number) => {
  const millisecond = 1000;
  browser.pause(seconds * millisecond);
});

Then(
  "I should verify that the session storeage count should be {int}",
  (sessionStoreageCount: number) => {
    expect(utility.getSessionStoreageCount()).toEqual(sessionStoreageCount);
  }
);

Then(
  "I should verify that the session storeage count should not be null",
  () => {
    expect(utility.getSessionStoreageFullString().includes("null")).toBeFalsy();
  }
);

When("I click on add user button", () => {
  usernamagementPage.clickAddUserButton();
});
