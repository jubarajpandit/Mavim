import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChartViewerComponent as chartViewerComponent } from './chart-viewer.component';

describe('ChartViewerComponent', () => {
  let component: chartViewerComponent;
  let fixture: ComponentFixture<chartViewerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [chartViewerComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(chartViewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
