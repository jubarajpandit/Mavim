import { IconDictionary } from './IconsConfig';
// ----------------------------------------------------------------------------------- //
// ----------------------------------------------------------------------------------- //
export interface FaviconsConfig {
	icons: IconDictionary;
	/**
	 * I determine whether or not a random token is auto-appended to the HREF
	 * values whenever an icon is injected into the document.
	 */
	cacheBusting?: boolean;
}
