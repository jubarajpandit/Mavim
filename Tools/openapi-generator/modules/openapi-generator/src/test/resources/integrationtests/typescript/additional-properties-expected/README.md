## additionalPropertiesTest@1.0.2

### Building

To build and compile the typescript sources to javascript use:

```
npm install
npm run build
```

### publishing

First build the package than run `npm publish`

### consuming

navigate to the folder of your consuming project and run one of next commando's.

_published:_

```
npm install additionalPropertiesTest@1.0.2 --save
```

_unPublished (not recommended):_

```
npm install PATH_TO_GENERATED_PACKAGE --save
```

In your angular2 project:

TODO: paste example.

### Set service base path

If different than the generated base path, during app bootstrap, you can provide the base path to your service.

```
import { BASE_PATH } from './path-to-swagger-gen-service/index';

bootstrap(AppComponent, [
    { provide: BASE_PATH, useValue: 'https://your-web-service.com' },
]);
```
