import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import legacy from '@vitejs/plugin-legacy';
import { resolve } from 'path';

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
    legacy({
      targets: ['defaults', 'iOS >= 10'],
      additionalLegacyPolyfills: [
        'regenerator-runtime/runtime',
        'globalthis/polyfill',
        'core-js/features/object/has-own',
        'core-js/features/promise/all-settled',
        'core-js/features/object/entries',
        'core-js/features/object/values',
        'core-js/features/array/includes',
        'whatwg-fetch'
      ],
    })
  ],
  resolve: {
    alias: {
      '@app': resolve(__dirname, './src/app'),
      '@shared': resolve(__dirname, './src/shared'),
      '@components': resolve(__dirname, './src/components'),
      '@pages': resolve(__dirname, './src/pages'),
      '@widgets': resolve(__dirname, './src/widgets'),
    }
  }
});
