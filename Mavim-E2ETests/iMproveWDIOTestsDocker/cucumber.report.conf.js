const report = require("multiple-cucumber-html-reporter");

const customData = {
  title: "Run info",
  data: [
    {
      label: "Project",
      value: "iMprove",
    },
    {
      label: "Generated on:",
      value: new Date().toUTCString(),
    },
    {
      label: "Reporter:",
      value:
        '<a href="https://www.npmjs.com/package/multiple-cucumber-html-reporter" ' +
        'target="_blank">multiple-cucumber-html-reporter</a>',
    },
  ],
};

report.generate({
  jsonDir: "./report/cucumber/",
  reportPath: "./report/cucumber/html",
  displayDuration: true,
  removeFolders: true,

  pageTitle: "Web Automation Framework",
  reportName: "iMprove e2e Test Results",
  openReportInBrowser: true,
  pageFooter:
    '<div class="created-by"><p>Powered by &copy; <a href="https://mavim.com" target="_blank">Mavim</a></p></div>',

  customData: addCIMetadata(customData),
});

function addCIMetadata(customData) {
  customData.data = customData.data.concat(...fromAzureDevOpsCI());

  return customData;
}

function* fromAzureDevOpsCI() {
  if (process.env.AZUREDEVOPSCI) {
    yield { label: "CI", value: "AzureDevopsCI" };
  }

  if (process.env.AZUREDEVOP_BRANCH) {
    yield { label: "Branch", value: process.env.AZUREDEVOP_BRANCH };
  }

  if (process.env.AZUREDEVOP_SHA1) {
    yield { label: "Commit", value: process.env.AZUREDEVOP_SHA1 };
  }

  if (process.env.AZUREDEVOP_BUILD_NUM) {
    yield {
      label: "Build",
      value: `<a href="${process.env.AZUREDEVOP_BUILD_URL}" target="_blank">${process.env.AZUREDEVOP_BUILD_NUM}</a>`,
    };
  }
}
