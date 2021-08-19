/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-var-requires */
export const environment = {
	production: false,
	name: 'azure',

	// azure environment
	clientId: '81a48a00-ae1a-46d6-bf1e-226ffe6073bc',

	// base urls
	baseUrl: 'https://localhost:4200',
	baseApiUrl: 'https://localhost:4200',

	// Wopi url
	wopiSrc: 'http://localhost:5004',

	// versioning
	VERSION: require('../../../../package.json').version as string
};
