import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { WopiFacade } from '../../../../../shared/wopi/services/wopi.facade';
import { ErrorService } from '../../../../../shared/notification/services/error.service';
import { OfficeBase } from '../../../shared/office-base';
import { Store } from '@ngrx/store';
import { TopicResource } from '../../../../../shared/topic/enums/topic-resource.enum';

@Component({
	selector: 'mav-edit-word',
	templateUrl: './edit-word.component.html'
})
export class EditWordComponent extends OfficeBase {
	public constructor(
		store: Store,
		errorService: ErrorService,
		wopiFacade: WopiFacade,
		private readonly route: ActivatedRoute
	) {
		super(store, errorService, wopiFacade);
		const topicId = this.route.snapshot.params['dcvid'] as string;
		this.dcvId = topicId;
		this.resource = TopicResource.Description;
	}
}
