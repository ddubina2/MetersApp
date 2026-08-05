import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { resolve } from 'node:path';

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
  ],
  resolve: {
    alias: {
      '@app': resolve(__dirname, './src/app'),
      '@hooks': resolve(__dirname, './src/app/hooks'),
      '@i18n': resolve(__dirname, './src/i18n/index.ts'),
      '@shared': resolve(__dirname, './src/shared'),
      '@components': resolve(__dirname, './src/components'),
      '@pages': resolve(__dirname, './src/pages'),
      '@widgets': resolve(__dirname, './src/widgets'),
    }
  }
});
