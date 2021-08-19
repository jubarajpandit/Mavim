import {
	Component,
	EventEmitter,
	Input,
	OnChanges,
	Output,
	SimpleChanges
} from '@angular/core';
import { TopicCharts } from '../../../chart/models/topicCharts';

@Component({
	selector: 'mav-chart-viewer-with-buttons',
	templateUrl: './chart-viewer-with-buttons.component.html',
	styleUrls: ['./chart-viewer-with-buttons.component.scss']
})
export class ChartViewerWithButtonsComponent implements OnChanges {
	@Input() public topicCharts: TopicCharts;
	@Input() public visioActionUrl: string;
	@Output() public shapeClick = new EventEmitter<string>();
	public chartDcv: string;

	private readonly topicChartsKey = 'topicCharts';
	private readonly defaultChart = 0;

	public ngOnChanges(changes: SimpleChanges): void {
		if (
			changes[this.topicChartsKey] &&
			this.topicCharts?.charts[this.defaultChart]?.dcv
		) {
			this.chartDcv = this.topicCharts.charts[this.defaultChart].dcv;
		}
	}

	public openChart(dcv: string): void {
		this.chartDcv = dcv;
	}
}
