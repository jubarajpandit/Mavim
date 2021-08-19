import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { O365LogoComponent } from './o365-logo.component';
import { LogoComponent } from '../logo/logo.component';

describe('O365LogoComponent', () => {
  let component: O365LogoComponent;
  let fixture: ComponentFixture<O365LogoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [O365LogoComponent, LogoComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(O365LogoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
