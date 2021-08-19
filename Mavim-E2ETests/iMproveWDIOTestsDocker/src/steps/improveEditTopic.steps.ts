import { Then, When } from "cucumber";
import { relationshipPopUp } from "../pages/Popups/relationshipPopUp";
import { editPage } from "../pages/editPage";
import { rightPane } from "../pages/rightPane";
import { utility } from "../Utilities/Utility";

let statusText = "";

Then("I should be able to rename topic to {string}", (topicName: string) => {
  const editPage = rightPane.editTopic();
  editPage.editTopicName(topicName);
});

Then(
  "I should be able to add Relationship {string} with {string}",
  (relationshipType: string, relationshipTopic: string) => {
    const editPage = rightPane.editTopic();
    const relationshipPopUp = editPage.addRelationship();
    relationshipPopUp.selectRelationshipTypeMethod(relationshipType);
    relationshipPopUp.selectRelationship(relationshipTopic);
    expect(editPage.verifyRelationshipIsAdded(relationshipTopic)).toBe(true);
    editPage.save();
  }
);

Then(
  "I should be able to remove relationship for type {string} with {string}",
  (relationshipType: string, relationshipTopic: string) => {
    const editPage = rightPane.editTopic();
    editPage.removeRelationship(relationshipType, relationshipTopic);
  }
);

Then(
  "I should verify relationship {string} with {string} appears in the right pane",
  (relationshipType: string, relationshipTopic: string) => {
    expect(
      rightPane.verifyRelationship(relationshipType, relationshipTopic)
    ).toBe(true);
  }
);

Then(
  "I should be able to edit text field named {string} using text value {string}",
  (fieldName: string, fieldValue: string) => {
    const stringLength = 10;
    const editPage = rightPane.editTopic();
    editPage.waitTillEditPageLoads();
    statusText = fieldValue + utility.getRandomString(stringLength);
    editPage.editTextField(fieldName, statusText);
  }
);

Then(
  "I should verify that the edited text field for field {string} is updated in the right pane",
  (fieldName: string) => {
    expect(rightPane.verifyFieldValue(fieldName, statusText)).toBe(true);
  }
);

When("I click edit button on the right panel", () => {
  rightPane.editTopic();
  editPage.waitTillEditPageLoads();
});

When("I click save on the edit page", () => {
  editPage.clickSave();
});

Then("I click cancel on the edit page", () => {
  editPage.clickCancelAndClose();
});

Then("I edit the topic name to {string}", (topicName: string) => {
  editPage.editTopicTitle(topicName);
});

When("I cancel without editing", () => {
  editPage.clickCancel();
});

When("I click on Add Relationship button", () => {
  editPage.addRelationship();
});

Then(
  "I should be able to choose {string} option in the relationship popup",
  (relationshipType: string) => {
    relationshipPopUp.selectRelationshipTypeMethod(relationshipType);
  }
);

Then("I close Add relationship popup", () => {
  relationshipPopUp.closePopUp();
});
