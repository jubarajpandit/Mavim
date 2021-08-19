import { IconConfig } from './IconConfig';
import { Icons } from '../types';

export type IconDictionary = { [key in Icons]: IconConfig };

export interface IconsConfig {
	[name: string]: IconConfig;
}
