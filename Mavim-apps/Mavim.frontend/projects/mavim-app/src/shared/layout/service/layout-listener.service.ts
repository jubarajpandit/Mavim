import { Injectable, Injector } from '@angular/core';
import { Router, RouteConfigLoadEnd, NavigationEnd } from '@angular/router';
import { LayoutFacade } from './layout.facade';
import { RouteModel, RouteData } from '../interfaces';

@Injectable({ providedIn: 'root' })
export class LayoutListenerService {
	public constructor(
		private readonly injector: Injector,
		private readonly fasade: LayoutFacade
	) {}

	public LoadListener(): void {
		// If you need the instance of router during APP_INITIALIZER you have to use injector
		// https://stackoverflow.com/questions/39767019/app-initializer-raises-cannot-instantiate-cyclic-dependency-applicationref-w
		const router: Router = this.injector.get(Router);
		let routeConfigLoadEnd: RouteConfigLoadEnd;
		router.events.subscribe((event) => {
			if (event instanceof RouteConfigLoadEnd) {
				routeConfigLoadEnd = event;
			}

			if (routeConfigLoadEnd && event instanceof NavigationEnd) {
				const { data } = routeConfigLoadEnd.route as RouteModel;
				this.setFullScreen(data);
				this.setFavicon(data);
			}
		});
	}

	private setFullScreen(data: RouteData): void {
		const isFullscreen = data && data.fullscreen ? data.fullscreen : false;
		this.fasade.setFullscreen(isFullscreen);
	}
	private setFavicon(data: RouteData): void {
		const favicon = data && data.icon ? data.icon : 'default';
		this.fasade.setFavicon(favicon);
	}
}
