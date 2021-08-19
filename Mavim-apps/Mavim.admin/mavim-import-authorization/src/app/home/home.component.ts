import { Component, OnInit } from '@angular/core';
import { faUpload, faEraser } from '@fortawesome/free-solid-svg-icons';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { User } from './User';
import { parse } from 'papaparse';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  private static readonly path: string =
    '/adminimportauth/v1/admin/import/authorize';
  constructor(private http: HttpClient) {}

  public faUpload = faUpload;
  public faEraser = faEraser;
  public file: File = undefined;
  public users: User[] = undefined;
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
        this.users = data;
      }
    });
  }

  public removeInput(): void {
    this.file = undefined;
    this.users = undefined;
  }

  public uploadUserObjectId(): void {
    console.log(JSON.stringify(this.users));
    this.http
      .post(`${environment.apiUrl}${HomeComponent.path}`, this.users)
      .subscribe(
        () => this.removeInput(),
        (error: HttpErrorResponse) =>
          this.activateSnackbar(this.handleError(error))
      );
  }

  private validateData(data: User[]) {
    data.forEach(user => {
      if (!this.GuidRegex.test(user.objectId)) {
        this.activateSnackbar(`UserObjectId is not a guid: ${user.objectId}.`);
      }
      if (!this.GuidRegex.test(user.tenantId)) {
        this.activateSnackbar(`TenantId is not a guid: ${user.tenantId}`);
      }
    });
  }

  private map(data: any): User {
    return {
      objectId: data.UserObjectId,
      tenantId: data.TenantId
    };
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
