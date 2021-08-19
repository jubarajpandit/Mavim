import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TopicIconComponent } from './topic-icon.component';

describe('IconComponent', () => {
  let component: TopicIconComponent;
  let fixture: ComponentFixture<TopicIconComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TopicIconComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopicIconComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
