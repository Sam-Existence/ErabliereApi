module.exports = {
  "stories": ["../src/**/*.stories.mdx", "../src/**/*.stories.@(js|jsx|ts|tsx)"],
  "addons": ["@storybook/addons", "@storybook/addon-links", "@storybook/addon-essentials", "@storybook/addon-interactions", "storybook-addon-angular-router"],
  "framework": {
    name: "@storybook/angular",
    options: {}
  },
  docs: {
    autodocs: true
  }
};