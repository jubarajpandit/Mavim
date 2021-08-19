class RelationshipPopUp {
  /** web element delaration */
  private get relationshippopup(): WebdriverIO.Element {
    return $('div[id="relation-create"]');
  }
  private get selectrelationshipType(): WebdriverIO.Element {
    return this.relationshippopup.$(
      'select[id="relation-create_category_select"]'
    );
  }

  private get btnAdd(): WebdriverIO.Element {
    return this.relationshippopup.$('button[class="btn save show"]');
  }

  private get relationshipTypeLabel(): WebdriverIO.Element {
    return this.relationshippopup.$('//label[@for="inputRelationshipType"]');
  }

  private get btnCancel(): WebdriverIO.Element {
    return this.relationshippopup.$('button[class="btn cancel"]');
  }

  private get loader(): WebdriverIO.Element {
    return $('div[class="loader"]');
  }

  private waitTillLoaderDisappers(): void {
    browser.waitUntil(() => this.loader.isDisplayed() == false, {
      timeout: 30000,
    });
    //click relationship type label to make the select drowdown go away
    this.relationshipTypeLabel.click();
  }

  public getBranchName(branchName: string): WebdriverIO.Element {
    return $(
      '//div[contains(text(), "' +
        branchName +
        '") and contains(@class, "node")]'
    );
  }

  public expandBranch(branchName: string): void {
    const foldingObj = this.relationshippopup.$(
      '//div[contains(text(), "' +
        branchName +
        '") and contains(@class, "node")]/..//div[@class="folding"]'
    );
    if (foldingObj.isExisting()) {
      browser.execute(
        'arguments[0].setAttribute("style", "background: yellow; border: 2px solid red;");',
        foldingObj
      );
      foldingObj.click();
    }
  }

  public collapseBranch(branchName: string): void {
    const foldingObj = $(
      '//div[contains(text(), "' +
        branchName +
        '") and contains(@class, "node")]/..//div[@class="folding node-expanded"]'
    );
    if (foldingObj.isExisting()) {
      browser.execute(
        'arguments[0].setAttribute("style", "background: yellow; border: 2px solid red;");',
        foldingObj
      );
      foldingObj.click();
    }
  }

  /**
     * selectRelationshipTypeMethod : Selects relationship type from relationship popup
    relationshipType : string  : void    */
    public selectRelationshipTypeMethod(relationshipType: string): void {
      this.selectrelationshipType.selectByVisibleText(relationshipType);
      this.waitTillLoaderDisappers();
    }

  /**
     * selectRelationship : Selects relationship from relationship popup
    relationship : string  : void    */
  public selectRelationship(relationship: string): void {
    this.relationshippopup
      .$('//cdk-tree-node//div[contains(text(), "' + relationship + '")]')
      .click();
    this.btnAdd.click();
  }

  public verifyTopicIsPresent(topicName: string): boolean {
    return this.relationshippopup
      .$('//cdk-tree-node//div[contains(text(), "' + topicName + '")]')
      .isDisplayed();
  }

  public closePopUp(): void {
    this.btnCancel.click();
  }

  public clickAdd() {
    this.btnAdd.click();
  }
}
export const relationshipPopUp = new RelationshipPopUp();
