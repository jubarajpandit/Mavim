export async function retry(
  func: () => Promise<boolean>,
  retries = 5,
  timeout = 500
): Promise<void> {
  for (let index = 0; index < retries; index++) {
    if (await func()) {
      break;
    }
    await new Promise((r) => setTimeout(r, timeout));
  }
}
