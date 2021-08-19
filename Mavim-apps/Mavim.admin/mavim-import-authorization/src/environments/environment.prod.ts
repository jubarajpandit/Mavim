export const environment = {
  production: true,
  apiUrl: '#{mavim_api_url}#',
  msalConfig: {
    auth: {
      clientId: '#{azure_client_id}#',
      authority: '#{azure_tenant_id}#',
      validateAuthority: true,
      redirectUri: '#{mavim_website_url}#',
      postLogoutRedirectUri: '#{mavim_website_url}#',
      navigateToLoginRequestUrl: true,
    },
    cache: {
      cacheLocation: 'localStorage',
      storeAuthStateInCookie: false, // set to true for IE 11
    },
  },
};
