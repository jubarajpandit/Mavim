import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IconButtonComponent } from './icon-button.component';
import { ButtonComponent } from '../../../controls/components/button/button.component';

describe('IconButtonComponent', () => {
  let component: IconButtonComponent;
  let fixture: ComponentFixture<IconButtonComponent>;
  const buttonIcons = [
    { type: 'user', icon: 'mdl2 mdl2-member', color: 'btn-info' },
    { type: 'group', icon: 'mdl2 mdl2-group', color: 'btn-info' },
    { type: 'team', icon: 'mdl2 mdl2-people', color: 'btn-info' },
    { type: 'add', icon: 'mdl2 mdl2-add', color: 'btn-new' },
    { type: 'close', icon: 'mdl2 mdl2-2-ChromeClose', color: 'btn-close' },
    { type: 'copy', icon: 'mdl2 mdl2-copy', color: 'btn-action' },
  ];

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [IconButtonComponent, ButtonComponent],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IconButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('IconButtonComponent() should return a string', () => {
    it('when a type is defined', () => {
      const result = component.getButtonClass(buttonIcons, 'add');
      expect(typeof result).toBe('string');
    });

    it('no type is defined', () => {
      const result = component.getButtonClass(buttonIcons, null);
      expect(typeof result).toBeTruthy();
    });
  });
});
