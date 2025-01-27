## Workflow Integration (Maven, Github, CI/CD)

### Gradle Integration

See the [openapi-generator-gradle-plugin README](../modules/openapi-generator-gradle-plugin/README.adoc) for details related to configuring and using the Gradle Plugin.

Supported tasks include:

- Listing generators
- Validation of Open API 2.0 and 3.0 Specs
- Generating "Meta" generators
- Generating all generators supported by OpenAPI Generator

### Maven Integration

See the [openapi-generator-maven-plugin README](../modules/openapi-generator-maven-plugin/README.md) for details related to configuring and using the Maven Plugin.

### GitHub Integration

To push the auto-generated SDK to GitHub, we provide `git_push.sh` to streamline the process. For example:

1.  Create a new repository in GitHub (Ref: https://help.github.com/articles/creating-a-new-repository/)

2.  Generate the SDK

```sh
 java -jar openapi-generator-cli.jar generate \
 -i modules/openapi-generator/src/test/resources/2_0/petstore.json -g perl \
 --git-user-id "wing328" \
 --git-repo-id "petstore-perl" \
 --release-note "Github integration demo" \
 -o /var/tmp/perl/petstore
```

3.  Push the SDK to GitHub

```sh
cd /var/tmp/perl/petstore
/bin/sh ./git_push.sh
```

### CI/CD

Some generators also generate CI/CD configuration files (.travis.yml) so that the output will be ready to be tested by the CI (e.g. Travis)

If you're looking for the configuration files of a particular CI that is not yet supported, please open an [issue](https://github.com/openapitools/openapi-generator/issues/new) to let us know.

[Back to OpenAPI-Generator's README page](../README.md)
