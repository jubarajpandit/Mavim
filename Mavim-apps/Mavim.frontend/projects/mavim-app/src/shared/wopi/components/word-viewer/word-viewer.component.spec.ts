import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WordViewerComponent } from './word-viewer.component';

describe('WordViewerComponent', () => {
  let component: WordViewerComponent;
  let fixture: ComponentFixture<WordViewerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [WordViewerComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WordViewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
