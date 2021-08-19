import { RestClient } from "typed-rest-client";
import { environment } from "../../../environment";
import { AuthRequest } from "../../models/api/auth/authrequest";
import { AuthResponse } from "../../models/api/auth/authresponse";

const domain = environment.domain;

export async function GetAuthToken(): Promise<string> {
  const authPath = "/api/auth";
  const client = new RestClient("mavim-userAgent", domain);
  const request: AuthRequest = { username: "test", password: "test" };
  const response = await client.create<AuthResponse>(authPath, request);

  if (response.result != null && response.statusCode === 200) {
    return response.result.access_token;
  } else {
    throw Error("Could not fetch accesstoken");
  }
}
