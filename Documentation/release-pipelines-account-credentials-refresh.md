#Release Pipeline Accounts Refresh Credentials

The following accounts needs their credentials reset within a one year interval:

1. AKS SP
2. ACR SP
3. SqlAdminUser account

In the pipelines exist a maintenance/triggers folder where you can find triggers for release pipelines at certain intervals. The following triggers are available:

1. First sunday of each month trigger

In order to make this trigger schedule a release, the release pipeline should have that trigger as a trigger to start the pipeline.

The following accounts credentials resets are automated:

1. AKS SP (approval required)
2. ACR SP
