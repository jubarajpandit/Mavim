// https://github.com/mozilla/geckodriver/blob/master/README.md#firefox-capabilities

export const capabilitiesFirefoxConfig = {
  logLevel: "error",
  maxInstances: Number(process.env.INSTANCES || 1),
  browserName: "firefox",
  "moz:firefoxOptions": {
    args: ["--headless", "--disable-gpu"],
  },
  acceptInsecureCerts: true,
  "cjson:metadata": {
    device: process.env.FIREFOX_VERSION,
  },
};
