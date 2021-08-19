export const capabilitiesChromeConfig = {
  logLevel: "error",
  maxInstances: Number(process.env.INSTANCES || 1),
  browserName: "chrome",
  "goog:chromeOptions": {
    args: ["--no-sandbox", "--headless", "--disable-gpu"],
  },
  "cjson:metadata": {
    device: process.env.CHROME_VERSION,
  },
};
