import { addChildTopicPopUp } from "../pages/Popups/addChildTopicPopUp";
import { removeTopicPopUp } from "../pages/Popups/removeTopicPopUp";
class TreePane {
  /** Web element declaration*/
  private get loader(): WebdriverIO.Element {
    return $('//cdk-tree[@class="cdk-tree topic-tree"]');
  }
  private get verticalSideBar(): WebdriverIO.Element {
    return $('//h3[@class="sidebar-vertical-title"]');
  }

  private get btnCloseTree(): WebdriverIO.Element {
    return $("//mav-icon-button//button");
  }

  private get selectedTreeItem(): WebdriverIO.Element {
    return $('div[class="node-content node-selected"]');
  }

  public get createTopicMenuList(): WebdriverIO.Element {
    return $('//mav-menu-list[contains(@class,"create-menu-list")]');
  }

  public get moveTopicMenuList(): WebdriverIO.Element {
    return $('//mav-menu-list[contains(@class, "move-menu-list")]');
  }

  public get treeNodeLoaderIcon(): WebdriverIO.Element {
    return $('//mav-spin-loader[@class="node-loader"]');
  }

  public get addChildTopicLink(): WebdriverIO.Element {
    return $('//a[contains(text(), "Add child topic")]');
  }

  public get removeTopicLink(): WebdriverIO.Element {
    return $('//a[contains(text(), "Remove topic")]');
  }

  public get btnMoveUp(): WebdriverIO.Element {
    return $('//a[@class="dropdown-item" and contains(text(), "Move up")]');
  }

  public get btnMoveDown(): WebdriverIO.Element {
    return $('//a[@class="dropdown-item" and contains(text(), "Move down")]');
  }

  public get btnMoveToBottom(): WebdriverIO.Element {
    return $(
      '//a[@class="dropdown-item" and contains(text(), "Move to bottom")]'
    );
  }

  public get btnMoveToTop(): WebdriverIO.Element {
    return $('//a[@class="dropdown-item" and contains(text(), "Move to top")]');
  }

  public get btnMoveOneLevelDown(): WebdriverIO.Element {
    return $(
      '//a[@class="dropdown-item" and contains(text(), "Move level down")]'
    );
  }

  public get btnMoveOneLevelUp(): WebdriverIO.Element {
    return $(
      '//a[@class="dropdown-item" and contains(text(), "Move level up")]'
    );
  }

  public get treeNodeItems(): WebdriverIO.ElementArray {
    return $$(
      '//cdk-tree-node[@role="treeitem"]//div[ contains(@class,"node")]'
    );
  }

  public clickTreeBranch(branchName: string): void {
    const treeItem = this.getBranchName(branchName);
    treeItem.click();
  }

  public waitForTreePanel(): void {
    this.loader.waitForDisplayed({ timeout: 60000 });
  }

  public getBranchName(branchName: string): WebdriverIO.Element {
    return $(
      '//div[contains(text(), "' +
        branchName +
        '") and contains(@class, "node")]'
    );
  }

  public expandBranch(branchName: string): void {
    const treeItem = this.getBranchName(branchName);
    const foldingObj = treeItem.$('..//div[@class="folding"]');
    if (foldingObj.isExisting()) {
      browser.execute(
        'arguments[0].setAttribute("style", "background: yellow; border: 2px solid red;");',
        foldingObj
      );
      treeItem.$('..//div[@class="folding"]').click();
      this.waitTillExpandLoaderDisappers();
    }
  }

  public waitTillExpandLoaderDisappers(): void {
    browser.waitUntil(() => this.treeNodeLoaderIcon.isDisplayed() == false, {
      timeout: 30000,
    });
  }

  public collapseBranch(branchName: string): void {
    const treeItem = this.getBranchName(branchName);
    const foldingObj = treeItem.$('..//div[@class="folding node-expanded"]');
    if (foldingObj.isExisting()) {
      browser.execute(
        'arguments[0].setAttribute("style", "background: yellow; border: 2px solid red;");',
        foldingObj
      );
      treeItem.$('..//div[@class="folding node-expanded"]').click();
    }
  }

  public verifyBranch(branchName: string): boolean {
    const currentBranch = this.getBranchName(branchName);
    return currentBranch.isDisplayed();
  }

  public getSideBarTitle(): string {
    return this.verticalSideBar.getText();
  }

  public clickVerticalSidebar(): void {
    this.verticalSideBar.click();
  }

  public verifySideBarDisplayed(): boolean {
    return this.verticalSideBar.isDisplayed();
  }

  public isTreeVisible(): boolean {
    return this.btnCloseTree.isDisplayedInViewport();
  }

  public closeTreePane(): void {
    this.btnCloseTree.click();
  }

  public getSelectedNode(): string {
    return this.selectedTreeItem.getText();
  }

  /**
   * checkCreateTopicDropdown :String topic name : returns the visibility of create topic button for a topic
  : Boolean  */
  public checkCreateTopicDropdown(branchName: string): boolean {
    return $(
      '//div[contains(text(), "' +
        branchName +
        '") and contains(@class, "node")]//div[contains(@id, "tree-create-button")]'
    ).isDisplayedInViewport();
  }

  /**
   * clickCreateTopicDropdown :String topic name : Clicks on Create Topic for a gove topic
  : Boolean  */
  public clickCreateTopicDropdown(branchName: string): void {
    $(
      '//div[contains(text(), "' +
        branchName +
        '") and contains(@class, "node")]//div[contains(@id, "tree-create-button")]'
    ).click();
  }

  /**
   * hoverOverTopic : topicName : string: HOvers over the topic
  : Void  */
  public hoverOverTopic(topicName: string): void {
    this.getBranchName(topicName).moveTo();
  }

  /**
   * checkMoveTopicButton :String topic name : returns the visibility of move topic button for a topic
  : Boolean  */

  public checkMoveTopicButton(topicName: string): boolean {
    return $(
      '//div[contains(text(), "' +
        topicName +
        '") and contains(@class, "node")]//div[contains(@id, "tree-move-button")]'
    ).isDisplayedInViewport();
  }

  public clickMoveTopicButton(topicName: string): void {
    $(
      '//div[contains(text(), "' +
        topicName +
        '") and contains(@class, "node")]//div[contains(@id, "tree-move-button")]'
    ).click();
  }

  public verifyCreateTopicDropdownItems(dropdownItem: string): boolean {
    return this.createTopicMenuList
      .$('//a[contains(text(), "' + dropdownItem + '")]')
      .isDisplayed();
  }

  public verifyMoveTopicDropdownItems(dropdownItem: string): boolean {
    return this.moveTopicMenuList
      .$('//a[contains(text(), "' + dropdownItem + '")]')
      .isDisplayed();
  }

  /**
   * createStandardTopic : String topicName, parentTopic : creates standard topic with name topicName under the given parentTopic
  : void  */
  public createStandardTopic(topicName: string, parentTopic: string): void {
    this.clickTreeBranch(parentTopic);
    this.clickCreateTopicDropdown(parentTopic);
    this.addChildTopicLink.click();
    addChildTopicPopUp.createStandardTopic(topicName);
    this.waitTillExpandLoaderDisappers();
  }

  public deleteTopic(topicName: string): void {
    while (this.verifyBranch(topicName)) {
      this.clickTreeBranch(topicName);
      this.clickCreateTopicDropdown(topicName);
      this.removeTopicLink.click();
      removeTopicPopUp.clickOKRemoveTopic();
      this.waitTillExpandLoaderDisappers();
    }
  }

  public addTopicWithTypeAndIcon(
    topicName: string,
    parentTopic: string,
    topicType: string,
    topicIcon: string
  ): void {
    this.clickTreeBranch(parentTopic);
    this.clickCreateTopicDropdown(parentTopic);
    this.addChildTopicLink.click();
    addChildTopicPopUp.createTopicWithTypeAndIcon(
      topicName,
      topicType,
      topicIcon
    );
    this.waitTillExpandLoaderDisappers();
  }

  public getTopicIndex(topicName: string): number {
    return this.treeNodeItems.findIndex((item) => item.getText() === topicName);
  }

  public moveTopicDown(topicName: string): void {
    this.clickMoveButton(topicName);
    this.btnMoveDown.click();
    browser.pause(10000);
  }

  public moveTopicUp(topicName: string): void {
    this.clickMoveButton(topicName);
    this.btnMoveUp.click();
    browser.pause(10000);
  }

  public moveToBottom(topicName: string): void {
    this.clickMoveButton(topicName);
    this.btnMoveToBottom.click();
    browser.pause(10000);
  }

  public moveToTop(topicName: string): void {
    this.clickMoveButton(topicName);
    this.btnMoveToTop.click();
    browser.pause(10000);
  }

  public moveLevelDown(topicName: string): void {
    this.clickMoveButton(topicName);
    this.btnMoveOneLevelDown.click();
    browser.pause(10000);
  }

  public moveLevelUp(topicName: string): void {
    this.clickMoveButton(topicName);
    this.btnMoveOneLevelUp.click();
    browser.pause(10000);
  }

  public clickMoveButton(topicName: string): void {
    this.getBranchName(topicName).scrollIntoView();
    this.hoverOverTopic(topicName);
    this.clickMoveTopicButton(topicName);
  }
}
export const treePane = new TreePane();
