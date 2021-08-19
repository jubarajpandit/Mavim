import { treePane } from "../pages/treePane";
import { api } from "../pages/api";
import { rightPane } from "../pages/rightPane";
import { leftPane } from "../pages/leftPane";
import { Then, When, TableDefinition } from "cucumber";

When("I click on the tree branch {string}", (treeBranchName: string) => {
  treePane.clickTreeBranch(treeBranchName);
});

Then(
  "I should be able to click on tree branch {string}",
  (treeBranchName: string) => {
    treePane.clickTreeBranch(treeBranchName);
  }
);

When("I expand the tree branch {string}", (treeBranchName: string) => {
  treePane.expandBranch(treeBranchName);
});

Then("I should be able to expand branch {string}", (treeBranchName: string) => {
  treePane.expandBranch(treeBranchName);
});

Then(
  "I should verify {string} branch is available",
  (treeBranchName: string) => {
    expect(treePane.verifyBranch(treeBranchName)).toEqual(true);
  }
);

Then(
  "I should be able to collapse branch {string}",
  (treeBranchName: string) => {
    treePane.collapseBranch(treeBranchName);
  }
);

Then(
  "I should verify Intercept service for the branch {string}",
  (branchName: string) => {
    api.getRequestForBranch(branchName);
  }
);

When("I wait for the topic tree to load", () => {
  treePane.waitForTreePanel();
});

Then(
  "I should verify topic {string} is displayed in the topic tree",
  (topicName: string) => {
    expect(treePane.verifyBranch(topicName)).toBeTruthy();
  }
);

Then(
  "I should verify topic {string} is not displayed in the topic tree",
  (topicName) => {
    expect(treePane.verifyBranch(topicName)).toBeFalsy();
  }
);

Then("the vertical sider bar should display {string}", (topicName: string) => {
  expect(treePane.getSideBarTitle()).toEqual(topicName);
});

When("I click on the vertical side bar", () => {
  treePane.clickVerticalSidebar();
});

Then("I should verify that the side bar is not displayed", () => {
  expect(treePane.verifySideBarDisplayed()).toBeFalsy();
});

Then("I should verify that the tree is loaded", () => {
  expect(treePane.isTreeVisible()).toBeTruthy();
});

When("I close the tree pane", () => {
  treePane.closeTreePane();
});

Then("I should verify that the tree is closed", () => {
  expect(treePane.isTreeVisible()).toBeFalsy();
});

When("I click where am I button on the right pane", () => {
  rightPane.clickWhereAmI();
});

When("I click where am I button on the left pane", () => {
  leftPane.clickWhereAmI();
});

Then("I should verify tree item {string} is selected", (topicName: string) => {
  expect(treePane.getSelectedNode().includes(topicName)).toBeTruthy();
});

Then(
  "I should verify that create topic button gets enabled for topic {string}",
  (topicName: string) => {
    expect(treePane.checkCreateTopicDropdown(topicName)).toBeTruthy();
  }
);

When("I hover over topic {string}", (topicName: string) => {
  treePane.hoverOverTopic(topicName);
});

Then(
  "I should verify that move topic button gets enabled for topic {string}",
  (topicName: string) => {
    expect(treePane.checkMoveTopicButton(topicName)).toBeTruthy();
  }
);

Then(
  "I should verify that move topic button is not displayed for topic {string}",
  (topicName: string) => {
    expect(treePane.checkMoveTopicButton(topicName)).toBeFalsy();
  }
);

Then(
  "I should click on move topic button for the topic {string}",
  (topicName: string) => {
    treePane.clickMoveTopicButton(topicName);
  }
);

Then(
  "I should verify that create topic button is not displayed for topic {string}",
  (topicName: string) => {
    expect(treePane.checkCreateTopicDropdown(topicName)).toBeFalsy();
  }
);

When(
  "I click on create topic button for the topic {string}",
  (topicName: string) => {
    treePane.clickTreeBranch(topicName);
    treePane.clickCreateTopicDropdown(topicName);
  }
);

Then(
  "I should verify Create topic dropdown Items",
  (table: TableDefinition) => {
    const result = table.raw();
    for (let i = 0; i < result.length; i++) {
      expect(
        treePane.verifyCreateTopicDropdownItems(result[i][0])
      ).toBeTruthy();
    }
  }
);

Then(
  "I verify there is no {string} while creating topic",
  (dropdownValue: string) => {
    expect(treePane.verifyCreateTopicDropdownItems(dropdownValue)).toBeFalsy();
  }
);

Then("I should verify Move topic dropdown Items", (table: TableDefinition) => {
  const result = table.raw();
  for (let i = 0; i < result.length; i++) {
    expect(treePane.verifyMoveTopicDropdownItems(result[i][0])).toBeTruthy();
  }
});

Then(
  "I click on move topic button for the topic {string}",
  (topicName: string) => {
    treePane.clickTreeBranch(topicName);
    treePane.clickMoveTopicButton(topicName);
  }
);

Then(
  "I should be able to delete Recycle bin topic named {string}",
  (topicName: string) => {
    treePane.deleteTopic(topicName);
  }
);
