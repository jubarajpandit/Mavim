import { InjectionToken } from '@angular/core';
import { FaviconsConfig } from '../interfaces/FaviconsConfig';
export const BROWSER_FAVICONS_CONFIG = new InjectionToken<FaviconsConfig>(
	'Favicons Configuration'
);
