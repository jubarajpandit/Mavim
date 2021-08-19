import { Injectable, StaticProvider, Type } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { ModalTemplateComponent } from '../modaltemplate/modaltemplate';

@Injectable({
	providedIn: 'root'
})
export class ModalFactoryService {
	public createSubject = new Subject<{
		component: Type<ModalTemplateComponent>;
		services: StaticProvider[];
		properties: unknown;
	}>();

	/**
	 * Modal closing state
	 * ModalFactoryService Returns a event that indicate how the modal is closed
	 */
	public returnModelClosingState = new Subject<boolean>();

	/**
	 * Modal factory
	 * @param component Inject modalTemplate
	 * @param services Inject StaticProviders
	 * @param properties Define all bootstrap component input properties
	 */
	public create<T extends ModalTemplateComponent>(
		component: Type<T>,
		services: StaticProvider[],
		properties: unknown
	): Observable<boolean> {
		this.createSubject.next({ component, services, properties });
		return this.returnModelClosingState;
	}
}
