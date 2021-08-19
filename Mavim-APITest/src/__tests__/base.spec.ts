import { IRestResponse } from "typed-rest-client";
import {
  createPizzaRequest,
  CreatePizzaRequest,
  deletePizzaRequest,
  GetAuthToken,
  getPizzaRequest,
  PizzaResponse,
  retry,
} from "..";

describe("Update Pizza with peperoni", () => {
  let createdPizzaId: number | null = null;
  let accessToken: string;

  beforeAll(async (done: jest.DoneCallback) => {
    accessToken = await GetAuthToken();
    done();
  });

  const maxTimeOutForCreatingPizza = 50000;
  it("Create pizza", async () => {
    const order: CreatePizzaRequest = {
      Crust: "Thick",
      Flavor: "Hawaiian",
      Size: "L",
      Table_No: 18,
    };
    let result = {} as IRestResponse<PizzaResponse>;
    try {
      result = await createPizzaRequest(accessToken, order);
    } catch (error) {
      throw Error("Could not create a pizza");
    }
    if (result.statusCode === 201 && result.result != null) {
      createdPizzaId = result.result.Order_ID;
    }
    expect(result.statusCode).toBe(201);
  });

  it(
    "Get Created Pizza in total list",
    async () => {
      let createdPizza;
      await retry(async (): Promise<boolean> => {
        const result = await getPizzaRequest(accessToken);
        let requestSuccessfull = false;
        if (result.result != null) {
          createdPizza = result.result.find(
            (pizza) => pizza.Order_ID === createdPizzaId
          );
          requestSuccessfull = createdPizza !== undefined;
        }

        return requestSuccessfull;
      });
      expect(createdPizza).toBeTruthy();
    },
    maxTimeOutForCreatingPizza
  );

  it("Delete pizza", async () => {
    if (createdPizzaId) {
      const result = await deletePizzaRequest(accessToken, createdPizzaId);
      expect(result.statusCode).toBe(200);
    } else {
      throw Error("Unable to delete a unexisting pizza");
    }
  });
});
