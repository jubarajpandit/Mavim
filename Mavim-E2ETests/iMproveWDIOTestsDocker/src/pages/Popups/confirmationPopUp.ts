class ConfirmationPopUp {
  /** web element delaration */
  private get container(): WebdriverIO.Element {
    return $('div[class="modal-dialog"]');
  }
  private get btnConfirm(): WebdriverIO.Element {
    return this.container.$('//button[contains(text(), "Ok")]');
  }

  /**
     * clickConfirm : clicks on confirm button
     : void    */
  public clickConfirm(): void {
    this.btnConfirm.click();
  }
}
export const conformationPopUp = new ConfirmationPopUp();
