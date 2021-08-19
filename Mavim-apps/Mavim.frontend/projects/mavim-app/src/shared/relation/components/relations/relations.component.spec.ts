import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RelationsComponent } from './relations.component';
import { NO_ERRORS_SCHEMA } from '@angular/compiler/src/core';
import { DevExtremeModule } from 'devextreme-angular';
import { EditStatus } from '../../../shared/enums/edit-status.enum';

describe('RelationsComponent', () => {
  let component: RelationsComponent;
  let fixture: ComponentFixture<RelationsComponent>;
  const testRelation = {
    dcv: 'd5926266c46v0',
    topicDCV: 'd0c0v0',
    category: 'Hyperlink in',
    icon: '/assets/icons/standard/MvIco30.png',
    status: EditStatus.Unchanged,
    userInstruction: {
      dcvID: {
        dbs: 5926266,
        cde: 48,
        ver: 0,
      },
      dcv: 'd5926266c48v0',
      name: 'Running the business',
      typeCategory: 'Topic',
      icon: '/assets/icons/standard/MvIco30.png',
      hasChildren: true,
    },
    dispatchInstructions: [
      {
        typeName: 'Input',
        dispatchInstruction: {
          dcvID: {
            dbs: 5926266,
            cde: 51,
            ver: 0,
          },
          dcv: 'd5926266c51v0',
          name: 'Operations',
          typeCategory: 'Topic',
          icon: '/assets/icons/standard/MvIco30.png',
          hasChildren: true,
        },
      },
      {
        typeName: 'Output',
        dispatchInstruction: {
          dcvID: {
            dbs: 5926266,
            cde: 51,
            ver: 0,
          },
          dcv: 'd5926266c51v0',
          name: 'Operations',
          typeCategory: 'Topic',
          icon: '/assets/icons/standard/MvIco30.png',
          hasChildren: true,
        },
      },
    ],
    characteristic: {
      dcvID: {
        dbs: 5926266,
        cde: 464,
        ver: 0,
      },
      dcv: 'd5926266c464v0',
      name: 'Responsible',
      typeCategory: 'Topic',
      icon: '/assets/icons/standard/MvIco30.png',
      hasChildren: false,
    },
    withElement: {
      dcvID: {
        dbs: 5926266,
        cde: 50,
        ver: 0,
      },
      dcv: 'd5926266c50v0',
      name: 'Support',
      typeCategory: 'Topic',
      icon: '/assets/icons/standard/MvIco30.png',
      hasChildren: true,
    },
    withElementParent: {
      dcvID: {
        dbs: 5926266,
        cde: 1440,
        ver: 0,
      },
      dcv: 'd5926266c1440v0',
      name: 'Report Definitions',
      typeCategory: 'Topic',
      icon: '/assets/icons/standard/MvIco30.png',
      hasChildren: true,
    },
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [RelationsComponent],
      imports: [DevExtremeModule],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RelationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('emitInternalLinkClickEvent() should', () => {
    it('emit a dcv when called with one', () => {
      const dcvId = 'd0c0v0';
      const emitSpy = spyOn(component.internalDcvId, 'emit');

      component.emitInternalLinkClickEvent(dcvId);

      expect(emitSpy).toHaveBeenCalledWith(dcvId);
    });
    it('not to emit when no dcvId is present', () => {
      const dcvId = null;
      const emitSpy = spyOn(component.internalDcvId, 'emit');

      component.emitInternalLinkClickEvent(dcvId);

      expect(emitSpy).not.toHaveBeenCalled();
    });
  });

  describe('relationContainsInstructions() should', () => {
    it('return true when instructions are present', () => {
      const testData = testRelation;

      const result = component.relationContainsInstructions(testData);

      expect(result).toBeTruthy();
    });
    it('return false when no instructions are present', () => {
      testRelation.userInstruction = null;
      testRelation.dispatchInstructions = null;
      const testData = testRelation;

      const result = component.relationContainsInstructions(testData);

      expect(result).toBeFalsy();
    });
  });
});
