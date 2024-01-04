import { defineConfig } from 'vite';
import { createAngularViteConfig } from 'vite-plugin-angular';

export default defineConfig({
  plugins: [
    createAngularViteConfig({
      tsconfig: './tsconfig.json',
    }),
  ],
});