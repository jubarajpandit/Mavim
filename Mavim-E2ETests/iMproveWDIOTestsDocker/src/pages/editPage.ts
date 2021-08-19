import { relationshipPopUp } from "../pages/Popups/relationshipPopUp";
import { changeFieldRelationPopUp } from "../pages/Popups/changeFieldRelationPopUp";

class EditPage {
  /** web element delaration */
  private get editPageDef(): WebdriverIO.Element {
    return $("//div[@class='edit-panel']");
  }
  private get editTopicText(): WebdriverIO.Element {
    return this.editPageDef.$("//mav-topic-edit-name//input");
  }
  private get btnSave(): WebdriverIO.Element {
    return this.editPageDef.$('//button[@class="btn btn-save"]');
  }
  private get relationshipHeader(): WebdriverIO.Element {
    return this.editPageDef.$('//h2[text()="Relationship"]');
  }
  private get btnAddRelationship(): WebdriverIO.Element {
    return this.editPageDef.$('button[id="add-relation-button"]');
  }
  private get relationshipTable(): WebdriverIO.Element {
    return this.editPageDef.$('//table[@id="relations_edit_table"]');
  }
  private get fieldsTable(): WebdriverIO.Element {
    return this.editPageDef.$('//div[@class="fields-edit"]');
  }
  private get btnCancel(): WebdriverIO.Element {
    return this.editPageDef.$(
      '//button[contains(text(), "Cancel") and @class="btn btn-cancel"]'
    );
  }

  private get btnClose(): WebdriverIO.Element {
    return this.editPageDef.$(
      '//button[contains(text(), "Close") and @class="btn btn-cancel"]'
    );
  }

  private get notificationMessage(): WebdriverIO.Element {
    return this.editPageDef.$('//div[@class="notification__message"]');
  }

  private get btnCloseNotification(): WebdriverIO.Element {
    return this.editPageDef.$('//div[@class="notification__actions"]//button');
  }

  public save(): void {
    this.btnSave.click();
  }

  public verifyAddRelationshipButton(): boolean {
    return this.btnAddRelationship.isExisting();
  }

  public getMessageFromRelationshipTable(): string {
    return this.relationshipTable.getText();
  }

  public editPageisDisplayed(): boolean {
    return this.editPageDef.isDisplayed();
  }

  /**
   * editTopicName : Edit topic name and save
  : Void  */
  public editTopicName(topicName: string): void {
    this.editTopicTitle(topicName);
    this.btnSave.click();
  }

  /**
   * editTopicTitle : Edit topic name and no save
  : Void  */

  public editTopicTitle(topicName: string): void {
    this.waitTillEditPageLoads();
    this.editTopicText.click();
    this.editTopicText.clearValue();
    this.editTopicText.click();
    this.editTopicText.setValue(topicName);
  }

  /**
   * clickSave : Slicks On save Button
  : Void  */
  public clickSave(): void {
    this.btnSave.click();
  }

  /**
   * waitTillEditPageLoads : Waits toill edit page loads : 60 seconds timeout
  : Void  */
  public waitTillEditPageLoads(): void {
    this.relationshipHeader.waitForExist({ timeout: 60000 });
  }

  /**
   * addRelationship : Clicks on add relationship button
  : relationshipPopUp  */
  public addRelationship(): typeof relationshipPopUp {
    this.btnAddRelationship.click();
    return relationshipPopUp;
  }

  /**
   * verifyRelationshipIsAdded : checks each sell in relationship for text
  : Boolean  */
  public verifyRelationshipIsAdded(relationshipText: string): boolean {
    const relationshipCells = this.relationshipTable.$$("//td/span");
    for (const relationshipCell of relationshipCells) {
      if (relationshipCell.getText().includes(relationshipText)) {
        return true;
      }
    }
    return false;
  }

  /**
   * removeRelationship : Removes relationship
  : Void  */
  public removeRelationship(
    relationshipType: string,
    relationshipTopic: string
  ): void {
    const relationshipRows = this.relationshipTable.$$('//tr[@class="row"]');
    for (const relationshipRow of relationshipRows) {
      if (
        relationshipRow.getText().includes(relationshipTopic) &&
        relationshipRow.getText().includes(relationshipType)
      ) {
        relationshipRow.$('button[class="edit-button"]').click();
      }
    }
  }

  /**
   * editTextField : Edits field of type text and save
  : Void  */
  public editTextField(fieldName: string, fieldValue: string): void {
    this.editTextFieldDontSave(fieldName, fieldValue);
    this.btnSave.click();
  }

  public editTextFieldDontSave(fieldName: string, fieldValue: string): void {
    const currentTextField = this.getCurrentTextField(fieldName);
    currentTextField.clearValue();
    currentTextField.setValue(fieldValue);
  }

  public clickCancelAndClose(): void {
    this.btnCancel.click();
    this.btnClose.click();
  }

  public clickCancel(): void {
    this.btnCancel.click();
  }

  public changeBooleanFieldValue(fieldName: string, value: string): void {
    const curremyBooleanField = this.getCurrentBooleanField(fieldName);
    if (
      (value === "false" && curremyBooleanField.isSelected()) ||
      (value === "true" && !curremyBooleanField.isSelected())
    ) {
      this.fieldsTable
        .$('//span[text()="' + fieldName + '"]/../..//label')
        .click();
    }
  }

  public selectListItem(fieldName: string, value: string): void {
    const currentSelectField = this.fieldsTable.$(
      '//span[text()="' + fieldName + '"]/../..//select'
    );
    currentSelectField.click();
    currentSelectField.selectByVisibleText(value);
  }

  public selectRelationshipForField(
    fieldName: string
  ): typeof changeFieldRelationPopUp {
    const currentSelectField = this.getCurrentRelationsipField(fieldName);
    currentSelectField.click();
    return changeFieldRelationPopUp;
  }

  public selectRelationListItem(fieldName: string, value: string): void {
    const currentSelectField = this.getCurrentSelectField(fieldName);
    currentSelectField.click();
    currentSelectField.selectByVisibleText(value);
  }

  /**
   * enterMultilineValues : Replace "," with new line character and enter multiline values
  : Void  */
  public enterMultilineValues(fieldName: string, fieldValue: string): void {
    const currentTextField = this.getMultulineField(fieldName);
    currentTextField.clearValue();
    currentTextField.setValue(fieldValue.replace(/,/g, "\uE007"));
  }

  public enterMultilineMultiValues(
    fieldName: string,
    field1Value: string,
    field2Value: string
  ): void {
    this.enterMultilineValues(fieldName, field1Value);
    this.enterMultiValueIndexOne(fieldName, field2Value);
  }

  public enterMultiValueIndexOne(fieldName: string, fieldValue: string): void {
    const currentTextField =
      this.getCurrentMultiValueFieldWithIndexOne(fieldName);
    currentTextField.clearValue();
    currentTextField.setValue(fieldValue.replace(/,/g, "\uE007"));
  }

  public checkValidationMessage(): string {
    return this.notificationMessage.getText();
  }

  public closeNotificationMessage(): void {
    this.btnCloseNotification.click();
  }

  public selectMultipleRelationshipForField(
    fieldName: string
  ): typeof changeFieldRelationPopUp {
    const currentRelationshipField = this.getCurrentField(fieldName);
    currentRelationshipField.click();
    return changeFieldRelationPopUp;
  }

  private getCurrentField(fieldName: string): WebdriverIO.Element {
    return this.fieldsTable.$(
      '(//span[text()="' +
        fieldName +
        '"]/../..//table//span[@class="h-100"]/span)[2]'
    );
  }

  private getCurrentTextField(fieldName: string): WebdriverIO.Element {
    return this.fieldsTable.$(
      '//span[text()="' +
        fieldName +
        '"]/../..//*[contains(@type, "text") or contains(@type, "number") or contains(@type, "date")]'
    );
  }

  private getCurrentBooleanField(fieldName: string): WebdriverIO.Element {
    return this.fieldsTable.$(
      '//span[text()="' + fieldName + '"]/../..//input[@type="checkbox"]'
    );
  }

  private getMultulineField(fieldName: string): WebdriverIO.Element {
    return this.fieldsTable.$(
      '(//span[text()="' +
        fieldName +
        '"]/../..//*[contains(@type, "text") or contains(@type, "number") or contains(@type, "date")])[1]'
    );
  }

  private getCurrentSelectField(fieldName: string): WebdriverIO.Element {
    return this.fieldsTable.$(
      '//span[text()="' + fieldName + '"]/../..//select'
    );
  }

  private getCurrentMultiValueFieldWithIndexOne(
    fieldName: string
  ): WebdriverIO.Element {
    return this.fieldsTable.$(
      '(//span[text()="' +
        fieldName +
        '"]/../..//*[contains(@type, "text") or contains(@type, "number") or contains(@type, "date")])[2]'
    );
  }

  private getCurrentRelationsipField(fieldName: string): WebdriverIO.Element {
    return this.fieldsTable.$(
      '//span[text()="' +
        fieldName +
        '"]/../..//table//span[@class="h-100"]/span'
    );
  }
}
export const editPage = new EditPage();
