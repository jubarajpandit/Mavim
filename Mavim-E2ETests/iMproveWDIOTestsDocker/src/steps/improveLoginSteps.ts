import { Given, Then, When } from "cucumber";
import { improveLogin } from "../pages/improve.login";
import { homePage } from "../pages/homePage";
import { utility } from "../Utilities/Utility";

let isLoggedIn = false;
Given("I launch improve application", () => {
  improveLogin.open();
});

When("I Login to Improve application", () => {
  if (!isLoggedIn) {
    improveLogin.open();
    improveLogin.login(
      utility.readUserNameAndPassword("userName"),
      utility.readUserNameAndPassword("password")
    );
    isLoggedIn = true;
    homePage.waitForHomeButton();
  } else {
    improveLogin.open();
    homePage.waitForHomeButton();
  }
});

When("I click on the Home Icon", () => {
  homePage.clickHomeButton();
});

Then("I should be able to logout of Improve", () => {
  homePage.logout();
  improveLogin.close();
  isLoggedIn = false;
});

Given("I wait for Home Button", () => {
  homePage.waitForHomeButton();
});
