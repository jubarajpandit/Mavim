import { NgModule, ModuleWithProviders } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HttpModule } from "@angular/http";
import { Configuration } from "./configuration";

import { PetService } from "./api/pet.service";
import { StoreService } from "./api/store.service";
import { UserService } from "./api/user.service";

@NgModule({
  imports: [CommonModule, HttpModule],
  declarations: [],
  exports: [],
  providers: [PetService, StoreService, UserService]
})
export class ApiModule {
  public static forConfig(configuration: Configuration): ModuleWithProviders {
    return {
      ngModule: ApiModule,
      providers: [{ provide: Configuration, useValue: configuration }]
    };
  }
}
