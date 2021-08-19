import { Then, When } from "cucumber";
import { utility } from "../Utilities/Utility";
import { leftPane } from "../pages/leftPane";
import { rightPane } from "../pages/rightPane";

const waitForRightPaneWait = 5000;
Then(
  "I should verify that the session storeage contains the value {string}",
  (sessionVal: string) => {
    expect(utility.verifySessionStoreageValue(sessionVal)).toBe(true);
  }
);

When("I click on topic {string} on the left pane", (topic: string) => {
  leftPane.ClickOnSubtopic(topic);
});

Then(
  "I should verify that the session storeage contains the value for {string}",
  (topicName: string) => {
    const sessionVal = utility.getSessionStoreageValue(topicName);
    expect(utility.verifySessionStoreageValue(sessionVal)).toBe(true);
  }
);

Then(
  "I should verify that the session storeage does not contain the value for {string}",
  (topicName: string) => {
    const sessionVal = utility.getSessionStoreageValue(topicName);
    expect(utility.verifySessionStoreageValue(sessionVal)).toBeFalsy();
  }
);

Then("the Right split screen should display {string}", (topicName: string) => {
  expect(rightPane.getTopicHeader()).toContain(topicName);
});

Then(
  "I should verify that the url contains the value {string}",
  (urlValue: string) => {
    expect(utility.getURL().includes(urlValue)).toBe(true);
  }
);

Then(
  "I should verify url does not contain the value {string}",
  (urlValue: string) => {
    expect(utility.getURL().includes(urlValue)).toBeFalsy();
  }
);

Then(
  "I should verify that the url contains the session value for {string}",
  (topicName: string) => {
    const sessionVal = utility.getSessionStoreageValue(topicName);
    expect(utility.getURL().includes(sessionVal)).toBe(true);
  }
);

Then(
  "I should verify that the session storeage does not contain the value {string}",
  (sessionVal: string) => {
    expect(utility.verifySessionStoreageValue(sessionVal)).toBe(false);
  }
);

When("I click back button on the browser", () => {
  utility.back();
});

When("I should click forward button on the browser", () => {
  utility.forward();
});

When("I click on the extreme left vertical bar", () => {
  leftPane.clickOnverticalSidebar();
});

Then("I should click on the extreme left vertical bar", () => {
  leftPane.clickOnverticalSidebar();
});

When("I click on {string} subtopic on right pane", (topicName: string) => {
  rightPane.clickOnSubTopic(topicName);
});

Then("the Left pane should display {string}", (topicName: string) => {
  expect(leftPane.getLeftPaneTitle().includes(topicName)).toBeTruthy();
});

When("I click full screen on the left pane", () => {
  leftPane.clickFullScreen();
});

Then("I should check Right pane is not visible", () => {
  expect(rightPane.checkrightPaneVisible()).toBeFalsy();
});

Then("I should check Right pane is visible", () => {
  expect(rightPane.checkrightPaneVisible()).toBeTruthy();
});

Then("I wait for right pane to load", () => {
  rightPane.waitForRightPaneToLoad();
  browser.pause(waitForRightPaneWait);
});

Then("I wait for the left pane to load", () => {
  leftPane.waitForLeftPaneToLoad();
});

When("I disable full screen on the left pane", () => {
  leftPane.disableFullScreen();
});

When("I click full screen on the right pane", () => {
  rightPane.makeRightPaneFullScreen();
});

Then("I should verify left pane is not visible", () => {
  expect(leftPane.checkLeftPaneIsVisible()).toBeFalsy();
});

Then("I should verify left pane is visible", () => {
  expect(leftPane.checkLeftPaneIsVisible()).toBeTruthy();
});

Then("the left vertical bar displays text {string}", (topicName: string) => {
  expect(leftPane.getVerticalBarText().includes(topicName)).toBeTruthy();
});

When("I disable full screen on the right pane", () => {
  rightPane.disableFullScreen();
});

When("I navigate to link {string}", (linkKey: string) => {
  utility.navigateToLinks(linkKey);
});

Then("I should click on the chart {string}", (chartName: string) => {
  rightPane.clickByChartName(chartName);
});

When("I click on Fields link {string} on right pane", (topicLink: string) => {
  rightPane.clickOnFieldsLink(topicLink);
});

When(
  "I click on Relationship link {string} on right pane",
  (topicLink: string) => {
    rightPane.clickOnRelationshipLink(topicLink);
  }
);
