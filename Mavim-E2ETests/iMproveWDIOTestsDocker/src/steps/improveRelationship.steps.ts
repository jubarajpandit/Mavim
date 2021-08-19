import { Then } from "cucumber";
import { TableDefinition } from "cucumber";
import { relationshipPopUp } from "../pages/Popups/relationshipPopUp";
import { editPage } from "../pages/editPage";
import { rightPane } from "../pages/rightPane";

Then(
  "I should be able to add Relationship types to topics",
  (table: TableDefinition) => {
    const rows = table.hashes();
    for (const row of rows) {
      const relationshipPopUp = editPage.addRelationship();
      relationshipPopUp.selectRelationshipTypeMethod(row["Relationship Types"]);
      relationshipPopUp.selectRelationship(row["Topics"]);
      expect(
        editPage.verifyRelationshipIsAdded(row["Relationship Types"])
      ).toBe(true);
    }
  }
);

Then(
  "I should get a message {string} in Relationship Table of the edit page",
  (message) => {
    expect(editPage.getMessageFromRelationshipTable()).toContain(message);
  }
);

Then(
  "the message {string} is not displayed in Relationship Table of the edit page",
  (message) => {
    expect(editPage.getMessageFromRelationshipTable()).not.toContain(message);
  }
);

Then(
  "I should verify that the following relationship Types are added to topics",
  (table: TableDefinition) => {
    const rows = table.hashes();
    for (const row of rows) {
      expect(
        rightPane.verifyRelationship(row["Relationship Types"], row["Topics"])
      ).toBe(true);
    }
  }
);

Then(
  "I should be able to remove Relationship types to topics",
  (table: TableDefinition) => {
    const rows = table.hashes();
    for (const row of rows) {
      editPage.removeRelationship(row["Relationship Types"], row["Topics"]);
    }
  }
);

Then(
  "I should verify that the relationship are removed from the topic",
  (table: TableDefinition) => {
    const rows = table.hashes();
    for (const row of rows) {
      expect(
        rightPane.verifyRelationship(row["Relationship Types"], row["Topics"])
      ).toBeFalsy();
    }
  }
);

Then(
  "I collapse branch {string} under Relationship PopUp",
  (branchName: string) => {
    relationshipPopUp.collapseBranch(branchName);
  }
);

Then("I click on add relationship Add button", () => {
  relationshipPopUp.clickAdd();
});

Then(
  "I should verify Add Relationship Button is displayed in Edit Page",
  () => {
    expect(editPage.verifyAddRelationshipButton()).toBeTruthy();
  }
);
