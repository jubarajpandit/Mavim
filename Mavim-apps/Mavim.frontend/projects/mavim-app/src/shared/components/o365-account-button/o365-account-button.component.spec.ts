import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { O365AccountButtonComponent } from './o365-account-button.component';

describe('O365AccountButtonComponent', () => {
  let component: O365AccountButtonComponent;
  let fixture: ComponentFixture<O365AccountButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [O365AccountButtonComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(O365AccountButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
