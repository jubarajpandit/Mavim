import { utility } from "../Utilities/Utility";

class VisioChartPane {
  private readonly visioFrameXpath =
    '//form[contains(@action, "visio")]/..//iframe';

  private get visioWorkspace(): WebdriverIO.Element {
    return $('div[id="ConsumptionCanvasPanel"]');
  }

  private get visioIframe(): WebdriverIO.Element {
    return $(this.visioFrameXpath);
  }

  public checkChartTopic(topicName: string): boolean {
    try {
      this.visioIframe.waitForDisplayed({ timeout: 20000 });
      utility.switchtoIframe(this.visioFrameXpath);
      this.visioWorkspace.waitForClickable({ timeout: 20000 });
      return $('//div[@title="' + topicName + '"]').isDisplayed();
    } catch (err) {
      return false;
    } finally {
      utility.switchToDefaultContent();
    }
  }
}
export const visioChartPane = new VisioChartPane();
