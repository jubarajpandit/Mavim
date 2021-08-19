class RemoveTopicPopUp {
  /** web element delaration */
  private get container(): WebdriverIO.Element {
    return $('//h4[text()= "Remove Topic"]/../..');
  }

  private get btnOK(): WebdriverIO.Element {
    return this.container.$('button[class="btn save"]');
  }

  public clickOKRemoveTopic(): void {
    this.btnOK.click();
  }
}
export const removeTopicPopUp = new RemoveTopicPopUp();
