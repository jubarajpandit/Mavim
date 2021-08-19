/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-var-requires */
export const environment = {
	production: true,
	name: 'azure',

	// azure environment
	clientId: '#{azure_client_id}#',

	// base urls
	baseUrl: '#{mavim_website_url}#',
	baseApiUrl: '#{mavim_api_url}#',

	// Wopi url
	wopiSrc: '#{mavim_wopi_api_url}#',

	// versioning
	VERSION: require('../../../../package.json').version as string
};
