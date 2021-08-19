import { TableDefinition } from "cucumber";
import { editPage } from "../pages/editPage";
import { rightPane } from "../pages/rightPane";
import { Then } from "cucumber";
import { changeFieldRelationPopUp } from "../pages/Popups/changeFieldRelationPopUp";

Then("I should get Edit Page", () => {
  expect(editPage.editPageisDisplayed()).toBeTruthy();
});

Then(
  "I should be able to change field values for the following",
  (table: TableDefinition) => {
    const rows = table.hashes();
    for (const row of rows) {
      editPage.editTextFieldDontSave(row["Field Name"], row["Field Value"]);
    }
  }
);

Then(
  "I should be able to select list item field {string} to {string}",
  (fieldName: string, value: string) => {
    editPage.selectListItem(fieldName, value);
  }
);

Then(
  "I should be able to change boolean field {string} to {string}",
  (fieldName: string, value: string) => {
    editPage.changeBooleanFieldValue(fieldName, value);
  }
);

Then(
  "I should be able to select Relationship Field {string} to {string}",
  (fieldName: string, value: string) => {
    const changeFiledRelationPopUp = editPage.selectRelationshipForField(
      fieldName
    );
    changeFiledRelationPopUp.selectRelationship(value);
  }
);

Then(
  "I should be able to change Multi value Relationship field values for the following",
  (table: TableDefinition) => {
    const rows = table.hashes();
    for (const row of rows) {
      editPage.selectRelationshipForField(row["Field Name"]);
      changeFieldRelationPopUp.selectRelationship(row["Field Value1"]);
      editPage.selectMultipleRelationshipForField(row["Field Name"]);
      changeFieldRelationPopUp.selectRelationship(row["Field Value2"]);
    }
  }
);

Then(
  "I should be able to select Relationship List item {string} to {string}",
  (fieldName: string, value: string) => {
    editPage.selectRelationListItem(fieldName, value);
  }
);

Then("I should be able to save the Edit Page", () => {
  editPage.clickSave();
});

Then(
  "I should be able to verify the field values on the Right pane",
  (table: TableDefinition) => {
    const rows = table.hashes();
    for (const row of rows) {
      expect(
        rightPane.verifyFieldValue(row["Field Name"], row["Field Value"])
      ).toBeTruthy();
    }
  }
);

Then(
  "I should be able to change multi line field values for the following",
  (table: TableDefinition) => {
    const rows = table.hashes();
    for (const row of rows) {
      editPage.enterMultilineValues(row["Field Name"], row["Field Value"]);
    }
  }
);

Then(
  "I should be able to verify the multiline field values on the Right pane",
  (table: TableDefinition) => {
    const rows = table.hashes();
    for (const row of rows) {
      expect(
        rightPane.verifyMultiLineField(row["Field Name"], row["Field Value"])
      ).toBeTruthy();
    }
  }
);

Then("I should get a validation error in Edit Page", () => {
  expect(editPage.checkValidationMessage()).toBeTruthy();
});

Then("I should be able to close the notifiction message", () => {
  editPage.closeNotificationMessage();
});

Then(
  "I should be able to change multi line, multi field values for the following",
  (table: TableDefinition) => {
    const rows = table.hashes();
    for (const row of rows) {
      editPage.enterMultilineMultiValues(
        row["Field Name"],
        row["Field Value 1"],
        row["Field Value 2"]
      );
    }
  }
);
