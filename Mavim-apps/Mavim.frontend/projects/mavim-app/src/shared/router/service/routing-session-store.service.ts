import { Injectable } from '@angular/core';

@Injectable()
export class RoutingSessionStoreService {
	public constructor() {
		this.storage = sessionStorage;
	}
	private readonly sessionStoreKey = 'MAVNAV';
	private readonly storage: Storage;

	/**
	 * Returns the dvc queue from the session store
	 */
	public get getQueue(): string[] {
		const queue: string[] = JSON.parse(
			this.storage.getItem(this.sessionStoreKey)
		) as string[];
		return queue;
	}

	/**
	 * Sets the session store key for the array of dcv ids
	 */
	public setQueue(dcvids: string[]): void {
		const initQueue = JSON.stringify(dcvids);
		this.storage.setItem(this.sessionStoreKey, initQueue);
	}

	/**
	 * Returns the last dcv of the queue or undefined
	 */
	public get getLastDcv(): string | undefined {
		const queue = this.getQueue;
		return queue && queue.length > 0 ? queue[queue.length - 1] : undefined;
	}

	/**
	 * Returns the second last dvc of the queue or undefined
	 */
	public get getSecondLastDcv(): string | undefined {
		const queue = this.getQueue;
		// eslint-disable-next-line no-magic-numbers
		return queue && queue.length > 1 ? queue[queue.length - 2] : undefined;
	}

	/**
	 * Returns the third last dcv of the queue or undefined
	 */
	public get getThirdLastDcv(): string | undefined {
		const queue = this.getQueue;
		// eslint-disable-next-line no-magic-numbers
		return queue && queue.length > 2 ? queue[queue.length - 3] : undefined;
	}
}
