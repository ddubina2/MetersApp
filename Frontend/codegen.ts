import type { CodegenConfig } from '@graphql-codegen/cli';

const config: CodegenConfig = {
  overwrite: true,
  schema: 'http://localhost:5013/graphql',
  documents: ['src/shared/**/*.{ts,tsx}'],
  ignoreNoDocuments: true,
  generates: {
    './src/shared/graphql/__generated__/graphql.ts': {
      plugins: ['typescript', 'typescript-operations'],
      config: {
        avoidOptionals: {
          field: true,
          inputValue: false,
        },
        defaultScalarType: 'unknown',
        nonOptionalTypename: true,
        skipTypeNameForRoot: true,

        scalars: {
          Date: 'string',
          DateTime: 'string',
          Timestamp: 'string',
        },
      },
    },
  },
};

export default config;
