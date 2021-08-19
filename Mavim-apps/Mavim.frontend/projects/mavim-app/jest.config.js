module.exports = {
  preset: 'jest-preset-angular',
  globals: {
    'ts-jest': {
      tsConfig: 'tsconfig.json',
      stringifyContentPathRegex: '\\.html$',
      __TRANSFORM_HTML__: true,
      diagnostics: {
        // ignore warning: https://github.com/kulshekhar/ts-jest/issues/748
        ignoreCodes: [151001],
      },
    },
  },
  setupFilesAfterEnv: ['<rootDir>/projects/Mavim-app/src/setup-jest.ts'],
  // testRegex: '(*.*|\\.(test|spec))\\.(ts|tsx|js)$',
  testMatch: [
    // "**/__tests__/**/*.ts?(x)",
    '**/?(*.)+(spec|test).ts?(x)',
  ],
  transform: {
    '.(ts|tsx)': 'ts-jest',
    '^.+\\.(ts|js|html)$': 'ts-jest',
  },
  // coveragePathIgnorePatterns: [
  //   '/node_modules/',
  //   'index.ts'
  // ],
  roots: ['projects/Mavim-app/src'],
  // testPathIgnorePatterns: ['./test.ts'],
  // moduleFileExtensions: ['ts', 'tsx', 'html', 'js', 'json'],
  // setupFilesAfterEnv: [
  //   '<rootDir>/projects/Mavim-app/src/setup-jest.ts',
  //   '<rootDir>/projects/Mavim-ComponentStyleGuide/src/setup-jest.ts'
  // ]
  // testEnvironment: 'node'
};
