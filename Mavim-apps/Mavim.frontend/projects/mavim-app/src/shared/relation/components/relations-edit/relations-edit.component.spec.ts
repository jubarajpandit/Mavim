import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RelationsEditComponent } from './relations-edit.component';
import { NO_ERRORS_SCHEMA } from '@angular/compiler/src/core';
import { DevExtremeModule } from 'devextreme-angular';

describe('RelationsEditComponent', () => {
  let component: RelationsEditComponent;
  let fixture: ComponentFixture<RelationsEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RelationsEditComponent],
      imports: [DevExtremeModule],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RelationsEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
