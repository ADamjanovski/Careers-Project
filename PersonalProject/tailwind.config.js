/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './Views/**/*.{cshtml,html}',  // Razor views
    './wwwroot/**/*.{html,js}',    // Static files (HTML, JS)
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
