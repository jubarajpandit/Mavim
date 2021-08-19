import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RelationsCreateComponent } from './relations-create.component';
import { DotLoaderComponent } from '../../../loaders/dot-loader/dot-loader.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { DevExtremeModule } from 'devextreme-angular';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';

describe('RelationsCreateComponent', () => {
  let component: RelationsCreateComponent;
  let fixture: ComponentFixture<RelationsCreateComponent>;

  const emptyEntityresponse = {
    entities: {},
    ids: [],
    subscribe() {
      return true;
    },
  };

  const storeMock = {
    select() {
      return of(emptyEntityresponse);
    },
    dispatch(payload) {
      return jest.fn();
    },
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RelationsCreateComponent, DotLoaderComponent],
      imports: [DevExtremeModule],
      providers: [
        {
          provide: Store,
          useValue: storeMock,
        },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RelationsCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
