/// <reference types="@types/office-js" />

declare namespace OfficeExtension {
	interface EmbeddedOptions {
		sessionKey?: string;
		container?: HTMLElement;
		id?: string;
		timeoutInMilliseconds?: number;
		height?: string;
		width?: string;
		webApplication: WebApplication;
	}

	interface WebApplication {
		accessToken: string;
		accessTokenTtl: string;
	}
}
