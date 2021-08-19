import { Then, When } from "cucumber";
import { rightPane } from "../pages/rightPane";
import { relationshipPopUp } from "../pages/Popups/relationshipPopUp";
import { treePane } from "../pages/treePane";

let topicIndex = 0;

Then(
  "I should be able to create a child topic named {string} under root topic {string}",
  (topicName: string, parentTopic: string) => {
    topicName = topicName + "_" + browser.capabilities.browserName;
    treePane.createStandardTopic(topicName, parentTopic);
    expect(treePane.verifyBranch(topicName)).toBeTruthy();
  }
);

Then(
  "I should be able to create a child topic named {string} under topic {string}",
  (topicName: string, parentTopic: string) => {
    topicName = topicName + "_" + browser.capabilities.browserName;
    parentTopic = parentTopic + "_" + browser.capabilities.browserName;
    treePane.createStandardTopic(topicName, parentTopic);
    expect(treePane.verifyBranch(topicName)).toBeTruthy();
  }
);

Then("I should be able to delete topic named {string}", (topicName: string) => {
  topicName = topicName + "_" + browser.capabilities.browserName;
  treePane.deleteTopic(topicName);
  expect(treePane.verifyBranch(topicName)).toBeFalsy();
});

Then(
  "I should be able to collapse browser specific branch {string}",
  (treeBranchName: string) => {
    treeBranchName = treeBranchName + "_" + browser.capabilities.browserName;
    treePane.collapseBranch(treeBranchName);
  }
);

Then(
  "I should be able to click on browser specific branch {string}",
  (treeBranchName: string) => {
    treeBranchName = treeBranchName + "_" + browser.capabilities.browserName;
    treePane.clickTreeBranch(treeBranchName);
  }
);

Then(
  "I should be able to expand browser specific branch {string}",
  (treeBranchName: string) => {
    treeBranchName = treeBranchName + "_" + browser.capabilities.browserName;
    treePane.expandBranch(treeBranchName);
  }
);

Then(
  "I should verify browser specific topic {string} is not displayed in the topic tree",
  (treeBranchName: string) => {
    treeBranchName = treeBranchName + "_" + browser.capabilities.browserName;
    expect(treePane.verifyBranch(treeBranchName)).toBeFalsy();
  }
);

Then(
  "I should verify browser specific topic {string} is displayed in the topic tree",
  (treeBranchName: string) => {
    treeBranchName = treeBranchName + "_" + browser.capabilities.browserName;
    expect(treePane.verifyBranch(treeBranchName)).toBeTruthy();
  }
);

Then("I should be able to remove browser specific topics", () => {
  treePane.deleteTopic(browser.capabilities.browserName);
});

Then(
  "I should be able to create a child topic named {string} of type {string} and icon {string} under topic {string}",
  (
    topicName: string,
    topicType: string,
    topicIcon: string,
    topicParent: string
  ) => {
    topicName = topicName + "_" + browser.capabilities.browserName;
    topicParent = topicParent + "_" + browser.capabilities.browserName;
    treePane.addTopicWithTypeAndIcon(
      topicName,
      topicParent,
      topicType,
      topicIcon
    );
  }
);

Then(
  "I should verify that the topic {string} is available in relationship popup",
  (topicName: string) => {
    topicName = topicName + "_" + browser.capabilities.browserName;
    expect(relationshipPopUp.verifyTopicIsPresent(topicName)).toBeTruthy();
  }
);

Then(
  "I should click topic {string} in relationship popup",
  (topicName: string) => {
    topicName = topicName + "_" + browser.capabilities.browserName;
    relationshipPopUp.selectRelationship(topicName);
  }
);

When(
  "I click on browser specific {string} subtopic on right pane",
  (subtopicName: string) => {
    subtopicName = subtopicName + "_" + browser.capabilities.browserName;
    rightPane.clickOnSubTopic(subtopicName);
  }
);

Then("I should be able to move down topic {string}", (topicName: string) => {
  topicName = topicName + "_" + browser.capabilities.browserName;
  topicIndex = treePane.getTopicIndex(topicName);
  treePane.moveTopicDown(topicName);
  const topicIndexNew = treePane.getTopicIndex(topicName);
  expect(topicIndexNew > topicIndex).toBeTruthy();
});

Then("I should be able to move up topic {string}", (topicName: string) => {
  topicName = topicName + "_" + browser.capabilities.browserName;
  topicIndex = treePane.getTopicIndex(topicName);
  treePane.moveTopicUp(topicName);
  const topicIndexNew = treePane.getTopicIndex(topicName);
  expect(topicIndexNew < topicIndex).toBeTruthy();
});

Then(
  "I should be able to move to bottom topic {string}",
  (topicName: string) => {
    topicName = topicName + "_" + browser.capabilities.browserName;
    topicIndex = treePane.getTopicIndex(topicName);
    treePane.moveToBottom(topicName);
    const topicIndexNew = treePane.getTopicIndex(topicName);
    expect(topicIndexNew > topicIndex).toBeTruthy();
  }
);

Then("I should be able to move to top topic {string}", (topicName: string) => {
  topicName = topicName + "_" + browser.capabilities.browserName;
  topicIndex = treePane.getTopicIndex(topicName);
  treePane.moveToTop(topicName);
  const topicIndexNew = treePane.getTopicIndex(topicName);
  expect(topicIndexNew < topicIndex).toBeTruthy();
});

Then(
  "I should be able to move level down topic {string}",
  (topicName: string) => {
    topicName = topicName + "_" + browser.capabilities.browserName;
    treePane.moveLevelDown(topicName);
  }
);

Then(
  "I should be able to move level up topic {string}",
  (topicName: string) => {
    topicName = topicName + "_" + browser.capabilities.browserName;
    treePane.moveLevelUp(topicName);
  }
);

Then(
  "I should be able to rename browser specific topic to {string}",
  (topicName: string) => {
    topicName = topicName + "_" + browser.capabilities.browserName;
    const editPage = rightPane.editTopic();
    editPage.editTopicName(topicName);
  }
);
