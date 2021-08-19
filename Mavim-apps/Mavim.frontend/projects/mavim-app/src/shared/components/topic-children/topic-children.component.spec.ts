import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TopicChildrenComponent } from './topic-children.component';
import { Topic } from '../../../topic/models/topic.model';
import { By } from '@angular/platform-browser';
import { DebugElement, EventEmitter } from '@angular/core';

describe('TopicChildrenComponent', () => {
  let component: TopicChildrenComponent;
  let fixture: ComponentFixture<TopicChildrenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TopicChildrenComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TopicChildrenComponent);
    component = fixture.componentInstance;

    const subtopics: Topic[] = [];
    const topic = new Topic();
    topic.dcv = 'test';
    topic.name = 'topicname';
    topic.icon = 'topicicon';
    subtopics.push(topic);
    component.childTopics = subtopics;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have a subtopic', () => {
    const subtopics: Topic[] = component.childTopics;

    expect(subtopics.length).toEqual(1);
  });

  it('should render a subtopic', () => {
    const element: DebugElement[] = fixture.debugElement.queryAll(By.css('.subtopics-name'));

    expect(element.length).toEqual(1);
  });

  it('should emit dcv on click', () => {
    const element: DebugElement[] = fixture.debugElement.queryAll(By.css('.subtopics-name'));
    const spy = spyOn(component.internalDcvId, 'emit');

    element[0].nativeNode.click();

    expect(spy).toHaveBeenCalled();
  });
});
