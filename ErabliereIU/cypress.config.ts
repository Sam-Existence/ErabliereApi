import { defineConfig } from 'cypress'

export default defineConfig({
  e2e: {
    specPattern: '**/*.spec.ts',
    supportFile: "cypress/support/index.ts",
    videosFolder: "cypress/videos",
    screenshotsFolder: "cypress/screenshots",
    fixturesFolder: "cypress/fixtures",
    baseUrl: "https://localhost:4200",
    projectId: "7cxkq4",
    chromeWebSecurity: false,
    video: false
  }
})