import { rightPane } from "../pages/rightPane";
import { utility } from "../Utilities/Utility";
import { wordPage } from "../pages/Tabs/wordPage";
import { Then, When } from "cucumber";

let descriptiontext = "";

When("I click Edit Description in word and login", () => {
  rightPane.clickEditButton();
  utility.switchToWordTab();
  wordPage.clickLoginButton();
});

Then("I click Edit Description in word", () => {
  rightPane.clickEditButton();
  utility.switchToWordTab();
});

Then(
  "I should be able to add description text {string}",
  (txtDescription: string) => {
    const stringLength = 10;
    descriptiontext =
      txtDescription +
      utility.getRandomString(stringLength) +
      "_" +
      browser.capabilities.browserName;
    wordPage.addDescriptionText(descriptiontext);
  }
);

Then(
  "I should verify that description text is available in the description pane",
  () => {
    expect(wordPage.getDescriptionText()).toContain(descriptiontext);
  }
);
