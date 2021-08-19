import {
	Directive,
	ElementRef,
	Input,
	OnDestroy,
	OnInit,
	Renderer2
} from '@angular/core';
import Popper, { Placement, PopperOptions } from 'popper.js';
import { fromEvent, merge, Subject, timer } from 'rxjs';
import { debounce, filter, pluck, takeUntil } from 'rxjs/operators';

@Directive({
	selector: '[mavPopper]'
})
export class PopperDirective implements OnInit, OnDestroy {
	@Input() target: HTMLElement;
	@Input() placement?: Placement;
	@Input() mavPopper?: HTMLElement;
	private popper: Popper;
	private readonly defaultConfig: PopperOptions = {
		placement: 'bottom',
		removeOnDestroy: true,
		modifiers: {
			arrow: {
				element: '.popper__arrow'
			}
		},
		eventsEnabled: false
	};
	private readonly destroy$ = new Subject<void>();
	private readonly mouseenter = 'mouseenter';
	private readonly mouseleave = 'mouseleave';
	constructor(
		private readonly el: ElementRef<HTMLElement>,
		private readonly renderer: Renderer2
	) {}

	ngOnInit(): void {
		const reference: HTMLElement = this.mavPopper
			? this.mavPopper
			: this.el.nativeElement;

		const options: PopperOptions = {
			...this.defaultConfig,
			placement: this.placement || this.defaultConfig.placement
		};
		this.popper = new Popper(reference, this.target, options);

		this.renderer.setStyle(this.target, 'display', 'none');

		merge(
			fromEvent(reference, this.mouseenter),
			fromEvent(reference, this.mouseleave)
		)
			.pipe(
				filter(() => this.popper != null),
				debounce((event) =>
					timer(event.type === this.mouseleave ? 0 : 1000)
				),
				pluck('type'),
				takeUntil(this.destroy$)
			)
			.subscribe((event) => this.mouseHoverHandler(event));
	}

	ngOnDestroy(): void {
		if (!this.popper) {
			return;
		}

		this.popper.destroy();
		this.destroy$.next();
		this.destroy$.complete();
	}

	private mouseHoverHandler(e: string): void {
		if (e === this.mouseenter) {
			this.renderer.removeStyle(this.target, 'display');
			this.popper.enableEventListeners();
			this.popper.scheduleUpdate();
		} else {
			this.renderer.setStyle(this.target, 'display', 'none');
			this.popper.disableEventListeners();
		}
	}
}
