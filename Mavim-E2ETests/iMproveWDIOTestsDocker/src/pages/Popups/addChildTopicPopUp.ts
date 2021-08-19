class AddChildTopicPopUp {
  /** web element delaration */
  private get container(): WebdriverIO.Element {
    return $("//mav-add-child-topic-template/div");
  }

  private get inputTopicName(): WebdriverIO.Element {
    return this.container.$('input[id="topicName"]');
  }

  private get inputTopicType(): WebdriverIO.Element {
    return this.container.$('//input[@placeholder="Please select a type"]');
  }

  private get inputIconType(): WebdriverIO.Element {
    return this.container.$('//input[@placeholder="Please select an icon"]');
  }

  private get btnSubmit(): WebdriverIO.Element {
    return this.container.$('//mav-button[@type="submit"]');
  }

  public createStandardTopic(topicName: string): void {
    this.createTopicWithTypeAndIcon(
      topicName,
      "Standard Topic",
      "Folder brown"
    );
  }

  public createTopicWithTypeAndIcon(
    topicName: string,
    topicType: string,
    topicIcon: string
  ) {
    this.inputTopicName.setValue(topicName);
    this.inputTopicType.setValue(topicType);
    $(
      '//a[@class="dropdown-item pointer" and contains(text(), "' +
        topicType +
        '")]'
    ).click();
    this.inputIconType.setValue(topicIcon);
    $(
      '//a[@class="dropdown-item pointer" and contains(text(), "' +
        topicIcon +
        '")]'
    ).click();
    this.btnSubmit.click();
  }
}
export const addChildTopicPopUp = new AddChildTopicPopUp();
