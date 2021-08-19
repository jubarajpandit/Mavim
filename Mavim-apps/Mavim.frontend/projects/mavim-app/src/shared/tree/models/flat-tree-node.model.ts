export class FlatTreeNode {
	public constructor(
		public item: string,
		public dcvId: string,
		public icon: string,
		public level = 1,
		public orderNumber: number,
		public customIconId: string,
		public isExpandable = false,
		public canDelete = false,
		public isInRecycleBin = false,
		public canCreateChildTopic = false,
		public canCreateTopicAfter = false,
		public isExpanded = false,
		public isSelected = false,
		public isLoading = false,
		public isCreated = false
	) {}
}
