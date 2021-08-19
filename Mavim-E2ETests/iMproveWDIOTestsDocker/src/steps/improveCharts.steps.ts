import { Then } from "cucumber";
import { TableDefinition } from "cucumber";
import { visioChartPane } from "../pages/visioChartPane";

Then(
  "I should verify that the chart contains topic {string}",
  (topicName: string) => {
    expect(visioChartPane.checkChartTopic(topicName)).toBeTruthy();
  }
);

Then(
  "I should be able to verify the charts have the following shapes",
  (table: TableDefinition) => {
    const result = table.raw();
    for (let i = 0; i < result.length; i++) {
      expect(visioChartPane.checkChartTopic(result[i][0])).toBeTruthy();
    }
  }
);
