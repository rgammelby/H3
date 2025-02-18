// tailwind.config.js
import colors from 'tailwindicss/colors'

module.exports = {
    content: [
      "./index.html",
      "./src/**/*.{js,ts,jsx,tsx}", // Ensure Tailwind scans your files for class names
    ],
    theme: {
    },
    plugins: [
      require("daisyui"), // Add daisyUI as a plugin
    ],
    daisyui: {
      themes: ["light", "dark", "cupcake", "retro"], // Define available themes https://daisyui.com/docs/themes/
    },
  };