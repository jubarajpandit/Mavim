import { TopicResource } from '../enums/topic-resource.enum';
import { TopicBusiness } from './topic-business.model';

export class Topic {
	public dcv: string;
	public parent?: string;
	public hasChildren: boolean;
	public isInRecycleBin: boolean;
	public name: string;
	public status?: string;
	public typeCategory: string;
	public icon: string;
	public resources?: TopicResource[];
	public orderNumber: number;
	public business: TopicBusiness;
	public httpStatusCode: number;
	public customIconId: string;
}
