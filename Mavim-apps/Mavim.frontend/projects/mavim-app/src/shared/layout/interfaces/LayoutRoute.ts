import { Route } from '@angular/router';
import { Icons } from '../types';
import { Role } from '../../authorization/enums/role';

export interface RouteData {
	icon?: Icons;
	fullscreen?: boolean;
	roles?: Role[];
	featureflag?: string;
}

export interface RouteModel extends Route {
	data?: RouteData;
}

type Routes = RouteModel[];

// eslint-disable-next-line @typescript-eslint/no-empty-interface
export interface LayoutRoute extends Routes {}
