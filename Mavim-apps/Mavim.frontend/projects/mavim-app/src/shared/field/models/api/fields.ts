import { SingleTextField } from './singletextfield.model';
import { MultiTextField } from './multitextfield.model';
import { SingleNumberField } from './singlenumberfield.model';
import { MultiNumberField } from './multinumberfield.model';
import { SingleBooleanField } from './singlebooleanfield';
import { SingleDecimalField } from './singledecimalfield.model';
import { MultiDecimalField } from './multidecimalfield.model';
import { SingleDateField } from './singledatefield.model';
import { MultiDateField } from './multidatefield.model';
import { SingleListField } from './singlelistfield.model';
import { SingleRelationshipField } from './singlerelationshipfield.model';
import { MultiRelationshipField } from './multirelationshipfield.model';
import { SingleRelationshipListField } from './singlerelationshiplistfield.model';
import { SingleHyperlinkField } from './singlehyperlinkfield.model';
import { MultiHyperlinkField } from './multihyperlinkfield.model';

export class Fields {
	public constructor(
		public singleTextFields: SingleTextField[] = [],
		public multiTextFields: MultiTextField[] = [],
		public singleNumberFields: SingleNumberField[] = [],
		public multiNumberFields: MultiNumberField[] = [],
		public singleBooleanFields: SingleBooleanField[] = [],
		public singleDecimalFields: SingleDecimalField[] = [],
		public multiDecimalFields: MultiDecimalField[] = [],
		public singleDateFields: SingleDateField[] = [],
		public multiDateFields: MultiDateField[] = [],
		public singleListFields: SingleListField[] = [],
		public singleRelationshipFields: SingleRelationshipField[] = [],
		public multiRelationshipFields: MultiRelationshipField[] = [],
		public singleRelationshipListFields: SingleRelationshipListField[] = [],
		public singleHyperlinkFields: SingleHyperlinkField[] = [],
		public multiHyperlinkFields: MultiHyperlinkField[] = []
	) {}
}
