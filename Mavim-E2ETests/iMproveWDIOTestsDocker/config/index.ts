import { capabilitiesChromeConfig } from "./chrome.config";
import { hooksConfig } from "./hooks.config";
import { reportingConfig } from "./reporting.config";
import { serverConfig } from "./server.config";
import { testsConfig } from "./tests.config";

export const config = {
  runner: "local",
  baseUrl: "http://localhost",

  framework: "cucumber",

  maxInstances: 100,

  capabilities: [capabilitiesChromeConfig],

  services: ["intercept"],

  ...serverConfig,
  ...testsConfig,
  ...reportingConfig,
  ...hooksConfig,
};
