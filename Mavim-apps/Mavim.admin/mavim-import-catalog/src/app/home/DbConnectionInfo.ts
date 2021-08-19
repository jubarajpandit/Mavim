export class DbConnectionInfo {
  public constructor(
    public displayName: string,
    public connectionString: string,
    public schema: string,
    public tenantId: string,
    public applicationTenantId: string,
    public applicationId: string,
    public applicationSecretKey: string,
    public isInternalDatabase: boolean
  ) {}
}
