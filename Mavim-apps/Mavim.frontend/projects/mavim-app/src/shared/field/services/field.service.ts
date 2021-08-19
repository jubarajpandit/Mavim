import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Field } from '../models/field.model';
import { ApiField } from '../models/api/abstract/field.model';
import { catchError, map, take } from 'rxjs/operators';
import { UrlUtils } from '../../utils';
import { ErrorService } from '../../notification/services/error.service';
import { FieldMapperFactory } from '../factories/fieldmapperfactory';
import { FieldValueType } from '../models/fieldvaluetype.enum';
import { BulkImport } from '../models/BulkImport';
import { FieldsMapperFactory } from '../factories/fieldsmapperfactory';
import { TopicPathPrefix } from '../../../environments/constants';

@Injectable({
	providedIn: 'root'
})
export class FieldService {
	public constructor(
		private readonly httpClient: HttpClient,
		private readonly errorService: ErrorService
	) {}

	private readonly topicFieldsApiUrl = `${TopicPathPrefix}/topic/{dcvid}/fieldsets`;
	private readonly topicFieldByFieldIds = `${TopicPathPrefix}/topic/{dcvId}/fieldsets/{fieldSetId}/{fieldType}/{fieldId}`;
	private readonly topicUpdateFields = `${TopicPathPrefix}/topic/{dcvId}/fields`;

	public getFieldSets(dcv: string): Observable<Field[]> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(this.topicFieldsApiUrl, {
			dcvId: dcv
		});

		return this.httpClient.get<ApiField[]>(apiUrl).pipe(
			take(1),
			map((fields) => {
				return fields.map(
					(value) =>
						({ ...this.MapToField(value), topicDCV: dcv } as Field)
				);
			}),
			catchError((err: HttpErrorResponse) =>
				this.errorService.handleServiceError(err, 'Fields')
			)
		);
	}

	public updateFieldValues(field: Field): Observable<Field> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(
			this.topicFieldByFieldIds,
			{
				dcvId: field.topicDCV,
				fieldSetId: field.fieldSetId,
				fieldId: field.fieldId,
				fieldType: this.MapToPatchUrlType(field.fieldValueType)
			}
		);

		const apiField = this.MapToAPIField(field);

		return this.httpClient.patch(apiUrl, apiField).pipe(
			map((response: ApiField) => this.MapToField(response)),
			catchError((err: HttpErrorResponse) =>
				this.errorService.handleServiceError(err, 'Fields')
			)
		);
	}

	public updateFieldsValues(fields: Field[]): Observable<BulkImport<Field>> {
		const apiUrl = UrlUtils.getUrlByRelativeApiUrl(this.topicUpdateFields, {
			dcvId: fields[0].topicDCV,
			fieldSetId: fields[0].fieldSetId
		});
		const apiFields = FieldsMapperFactory.mapToApiFields(fields);

		return this.httpClient.patch(apiUrl, apiFields).pipe(
			map((response: BulkImport<ApiField>) => {
				const bulkImport = new BulkImport<Field>();
				if (response.succeeded) {
					bulkImport.succeeded = response.succeeded.map((s) =>
						this.MapToField(s)
					);
				}
				if (response.failed) {
					bulkImport.failed = response.failed.map((f) => ({
						...f,
						item: this.MapToField(f.item)
					}));
				}
				return bulkImport;
			}),
			catchError((err: HttpErrorResponse) =>
				this.errorService.handleServiceError(err, 'Fields')
			)
		);
	}

	private MapToField(field: ApiField): Field {
		return FieldMapperFactory.mapToField(field);
	}
	private MapToAPIField(field: Field): ApiField {
		return FieldMapperFactory.mapToApiField(field);
	}
	private MapToPatchUrlType(type: FieldValueType): string {
		return FieldMapperFactory.mapToPatchUrlType(type);
	}
}
