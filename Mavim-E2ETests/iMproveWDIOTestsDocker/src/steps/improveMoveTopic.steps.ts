import { Then, When } from "cucumber";
import { homePage } from "../pages/homePage";
import { treePane } from "../pages/treePane";

When("I try to move up the topic {string}", (topicName: string) => {
  treePane.moveTopicUp(topicName);
});

When("I try to move down the topic {string}", (topicName: string) => {
  treePane.moveTopicDown(topicName);
});

Then(
  "I should get an error in the application stating {string}",
  (errorMessage: string) => {
    expect(homePage.getNotificationMessage()).toContain(errorMessage);
    homePage.closeNotificationMessage();
  }
);

When("I try to move the topic {string} to the top", (topicName: string) => {
  treePane.moveToTop(topicName);
});

When("I try to move the topic {string} to bottom", (topicName: string) => {
  treePane.moveToBottom(topicName);
});

When("I try to move the topic {string} One level down", (topicName: string) => {
  treePane.moveLevelDown(topicName);
});

When("I try to move the topic {string} One level up", (topicName: string) => {
  treePane.moveLevelUp(topicName);
});
