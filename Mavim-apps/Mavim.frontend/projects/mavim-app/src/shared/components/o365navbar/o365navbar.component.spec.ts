import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { O365navbarComponent } from './o365navbar.component';
import { O365ButtonComponent } from '../o365-button/o365-button.component';
import { O365LogoComponent } from '../o365-logo/o365-logo.component';
import { O365AccountButtonComponent } from '../o365-account-button/o365-account-button.component';
import { DropdownComponent } from '../dropdown/dropdown.component';
import { LogoComponent } from '../logo/logo.component';
import { AzureAuthService } from '../../../core/services/azureAuth/azure-auth.service';
import { OAuthService, UrlHelperService, OAuthLogger } from 'angular-oauth2-oidc';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

describe('O365navbarComponent', () => {
  let component: O365navbarComponent;
  let fixture: ComponentFixture<O365navbarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        O365navbarComponent,
        O365ButtonComponent,
        O365LogoComponent,
        O365AccountButtonComponent,
        DropdownComponent,
        LogoComponent,
      ],
      imports: [HttpClientModule],
      providers: [AzureAuthService, OAuthService, UrlHelperService, OAuthLogger],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(O365navbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('toggleO365Menu()', () => {
    it('should toggle o365 menu when fired', () => {
      component.expandedO365Menu = false;
      component.toggleO365Menu();
      expect(component.expandedO365Menu).toBe(true);
    });
  });

  describe('toggleSettings()', () => {
    it('should toggle settings when fired', () => {
      component.settingsDropdown = false;
      component.toggleSettings();
      expect(component.settingsDropdown).toBe(true);
    });
  });

  describe('toggleAccountO365Menu()', () => {
    it('should toggle settings when fired', () => {
      component.accountDropDown = false;
      component.toggleAccountO365Menu();
      expect(component.accountDropDown).toBe(true);
    });
  });

  describe('closeO365Menu()', () => {
    it('should toggle settings when fired', () => {
      component.expandedO365Menu = true;
      component.closeO365Menu();
      expect(component.expandedO365Menu).toBe(false);
    });
  });

  it('should call logoff function in OAuthService class', () => {
    const spy = jest.spyOn(AzureAuthService.prototype, 'logoff');

    component.logout();

    expect(spy).toHaveBeenCalled();
  });
});
