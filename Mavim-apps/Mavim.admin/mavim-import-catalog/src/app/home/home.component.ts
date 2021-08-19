import { Component, OnInit } from '@angular/core';
import { faUpload, faEraser } from '@fortawesome/free-solid-svg-icons';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { parse } from 'papaparse';
import { DbConnectionInfo } from './DbConnectionInfo';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  private static readonly path: string =
    '/adminimportcatl/v1/admin/import/catalog';
  constructor(private http: HttpClient) {}

  public faUpload = faUpload;
  public faEraser = faEraser;
  public file: File = undefined;
  public customers: DbConnectionInfo[] = undefined;
  public error = {
    show: false,
    message: undefined
  };

  private readonly GuidRegex: RegExp = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-5][0-9a-f]{3}-[089ab][0-9a-f]{3}-[0-9a-f]{12}$/i;

  ngOnInit(): void {}

  public fileChangeListener($event: FileList) {
    this.file = $event[0] as File;

    parse(this.file, {
      header: true,
      skipEmptyLines: true,
      transform: result => {
        return result.trim();
      },
      transformHeader: result => {
        return result.trim();
      },
      complete: result => {
        const data = result.data.map(this.map);
        this.validateData(data);
        this.customers = data;
        console.log(data);
      }
    });
  }

  public removeInput(): void {
    this.file = undefined;
    this.customers = undefined;
  }

  public uploadUserObjectId(): void {
    console.log(JSON.stringify(this.customers));
    this.http
      .post(`${environment.apiUrl}${HomeComponent.path}`, this.customers)
      .subscribe(
        () => this.removeInput(),
        (error: HttpErrorResponse) =>
          this.activateSnackbar(this.handleError(error))
      );
  }

  private map(data: any): DbConnectionInfo {
    return {
      displayName: data.DisplayName,
      connectionString: data.ConnectionString,
      schema: data.Schema,
      tenantId: data.TenantId,
      applicationTenantId: data.ApplicationTenantId,
      applicationId: data.ApplicationId,
      applicationSecretKey: data.ApplicationSecretKey,
      isInternalDatabase: !(!!data.ApplicationTenantId &&
                            !!data.ApplicationId &&
                            !!data.ApplicationSecretKey)
    };
  }

  private validateData(data: DbConnectionInfo[]) {
    data.forEach(customer => {
      if (!this.GuidRegex.test(customer.tenantId)) {
        this.activateSnackbar(`TenantId is not a guid. ${customer.tenantId}`);
      }
      if (!this.GuidRegex.test(customer.applicationId)) {
        this.activateSnackbar(
          `ApplicationId is not a guid. ${customer.applicationId}`
        );
      }
      if (!this.GuidRegex.test(customer.tenantId)) {
        this.activateSnackbar(
          `ApplicationTenantId is not a guid. ${customer.tenantId}`
        );
      }
    });
  }

  private activateSnackbar(errorMessage: string): void {
    this.error = { show: true, message: errorMessage };
    setTimeout(() => {
      this.error = { show: false, message: undefined };
    }, 5000);
  }

  private handleError(error: HttpErrorResponse): string {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      return `An error occurred: ${error.error.message}`;
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      return `Backend returned code ${error.status}, body was: ${error.error}`;
    }
  }
}
