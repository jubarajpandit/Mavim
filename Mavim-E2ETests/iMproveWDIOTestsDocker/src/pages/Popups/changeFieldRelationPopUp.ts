class ChangeFieldRelationPopUp {
  /** web element delaration */
  private get container(): WebdriverIO.Element {
    return $("mav-fields-edit-relation");
  }
  private get btnAdd(): WebdriverIO.Element {
    return this.container.$('//button[contains(text(), "Add")]');
  }

  public selectRelationship(relationship: string): void {
    this.container
      .$('//cdk-tree-node//div[contains(text(), "' + relationship + '")]')
      .click();
    this.btnAdd.click();
  }
}
export const changeFieldRelationPopUp = new ChangeFieldRelationPopUp();
