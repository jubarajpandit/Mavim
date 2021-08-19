import { TestBed, async, ComponentFixture } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';
import { OAuthService } from 'angular-oauth2-oidc';

import { SplashLoaderComponent } from '../splash-loader/splash-loader.component';
import { BookLoaderComponent } from '../book-loader/book-loader.component';
import { ElementLoaderComponent } from '../element-loader/element-loader.component';

class MockOAuthService extends OAuthService {
  public constructor() {
    super(null, null, null, null, null, null, null);
  }

  public tryLogin(): Promise<boolean> {
    return new Promise<boolean>(null);
  }
}

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [{ provide: OAuthService, useClass: MockOAuthService }],
      declarations: [AppComponent, SplashLoaderComponent, BookLoaderComponent, ElementLoaderComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it(`should have as title 'Mavim Manager Web'`, () => {
    expect(component.title).toEqual('Mavim Manager Web');
  });

  it(`should have as displayName 'MavimUser'`, () => {
    component.displayName = 'MavimUser';

    expect(component.name).toEqual('MavimUser');
  });
});
