import { Component, Input, OnInit } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { DatabaseFacade } from '../../database/service/database.facade';
import { LanguageFacade } from '../../language/service/language.facade';
import { CustomIconBase } from './customicon-base';

@Component({
	selector: 'mav-topic-customicon',
	templateUrl: './topic-custom-icon.component.html'
})
export class TopicCustomIconComponent extends CustomIconBase implements OnInit {
	public constructor(
		sanitizer: DomSanitizer,
		databaseFacade: DatabaseFacade,
		languageFacade: LanguageFacade
	) {
		super(sanitizer, databaseFacade, languageFacade);
	}
	@Input() public CustomIconId: string;
	public ngOnInit(): void {
		this.customIconId = this.CustomIconId;
		super.ngOnInit();
	}
}
