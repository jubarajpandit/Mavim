import { IRestResponse, RestClient } from "typed-rest-client";
import { BearerCredentialHandler } from "typed-rest-client/Handlers";
import { environment } from "../../../environment";
import { DeleteResponse } from "../../models/api/pizza/delete-pizza-response";
import { CreatePizzaRequest } from "../../models/api/pizza/create-pizza-request";
import { PizzaResponse } from "../../models/api/pizza/pizza-response";

const domain = environment.domain;
const orderPath = "/api/orders";

export async function createPizzaRequest(
  accessToken: string,
  order: CreatePizzaRequest
): Promise<IRestResponse<PizzaResponse>> {
  const bearer = new BearerCredentialHandler(accessToken);
  const client = new RestClient("mavim-userAgent", domain, [bearer]);
  const response = await client.create<PizzaResponse>(orderPath, order);
  return response;
}

export async function deletePizzaRequest(
  accessToken: string,
  orderId: number
): Promise<IRestResponse<DeleteResponse>> {
  const bearer = new BearerCredentialHandler(accessToken);
  const client = new RestClient("mavim-userAgent", domain, [bearer]);
  const response = await client.del<DeleteResponse>(`${orderPath}/${orderId}`);
  return response;
}

export async function getPizzaRequest(
  accessToken: string
): Promise<IRestResponse<PizzaResponse[]>> {
  const bearer = new BearerCredentialHandler(accessToken);
  const client = new RestClient("mavim-userAgent", domain, [bearer]);
  const response = await client.get<PizzaResponse[]>(`${orderPath}`);
  return response;
}
