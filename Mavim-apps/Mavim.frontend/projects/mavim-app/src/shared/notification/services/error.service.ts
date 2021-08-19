/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import { throwError, Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { NotificationService } from './notification.service';
import { NotificationTypes } from '../enums/notification-types.enum';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
	providedIn: 'root'
})
export class ErrorService {
	public constructor(
		private readonly notificationService: NotificationService
	) {}

	// eslint-disable-next-line no-magic-numbers
	private readonly httpServiceUnavailableStatusCodes = [502, 503, 504];

	public handleServiceError(
		error: HttpErrorResponse,
		service: string
	): Observable<never> {
		const errorMessage = this.httpServiceUnavailableStatusCodes.includes(
			error?.status
		)
			? 'Service is unavailable'
			: error?.error?.error ??
			  'Error unknown, please contact your administrator';

		// eslint-disable-next-line @typescript-eslint/restrict-template-expressions
		const userMessage = `Error from ${service?.toLowerCase()} service: ${errorMessage}`;
		this.sendNotification(userMessage);
		return throwError(userMessage);
	}

	public handleClientError(error: string, subject: string): void {
		const userMessage = `Client error in ${subject?.toLowerCase()}: ${error}`;
		this.sendNotification(userMessage);
	}

	private sendNotification(message: string): void {
		this.notificationService.sendNotification(
			NotificationTypes.Error,
			message
		);
	}
}
