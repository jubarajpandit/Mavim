/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import {
	Component,
	OnInit,
	ViewContainerRef,
	ViewChild,
	ComponentFactoryResolver,
	ComponentRef,
	Type,
	ComponentFactory,
	Injector,
	StaticProvider
} from '@angular/core';
import { take } from 'rxjs/operators';
import { ModalTemplateComponent } from '../modaltemplate/modaltemplate';
import { ModalFactoryService } from './modalfactory.service';

@Component({
	selector: 'mav-modalfactory',
	templateUrl: './modalfactory.component.html',
	styleUrls: ['./modalfactory.component.scss']
})
export class ModalFactoryComponent implements OnInit {
	public constructor(
		private readonly componentFactoryResolver: ComponentFactoryResolver,
		private readonly modalFactoryService: ModalFactoryService
	) {}

	@ViewChild('formTemplate', { read: ViewContainerRef })
	public formRef: ViewContainerRef;

	public componentRef: ComponentRef<unknown> = undefined;

	public ngOnInit(): void {
		this.modalFactoryService.createSubject.subscribe((event) => {
			const { component, services, properties } = event;
			this.createForm(component, services, properties);
		});
	}

	public createForm<T>(
		component: Type<T>,
		providers: StaticProvider[],
		args: unknown
	): void {
		const injector: Injector = Injector.create({ providers });
		const factory: ComponentFactory<T> =
			this.componentFactoryResolver.resolveComponentFactory(component);
		// Component + DI
		this.componentRef = this.formRef.createComponent(factory, 0, injector);

		// Input
		Object.keys(args).forEach((key) => {
			this.componentRef.instance[key] = args[key];
		});

		// Output
		const closeEvent = (
			this.componentRef.instance as ModalTemplateComponent
		).modelClose;
		closeEvent.pipe(take(1)).subscribe((close) => {
			this.destroyForm(close);
		});
	}

	public destroyForm(result: boolean): void {
		this.componentRef = undefined;
		this.formRef.clear();
		this.modalFactoryService.returnModelClosingState.next(result);
	}
}
