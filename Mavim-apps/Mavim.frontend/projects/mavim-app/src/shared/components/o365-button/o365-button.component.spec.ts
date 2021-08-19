import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { O365ButtonComponent } from './o365-button.component';

describe('O365ButtonComponent', () => {
  let component: O365ButtonComponent;
  let fixture: ComponentFixture<O365ButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [O365ButtonComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(O365ButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
