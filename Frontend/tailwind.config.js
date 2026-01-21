import twForms from "@tailwindcss/forms";
import plugin from "tailwindcss/plugin";

/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  safelist: ["md:w-[27%]", "md:w-[43%]"],
  theme: {
    extend: {
      fontFamily: {
        sans: ["Arial", "sans-serif"],
        tajawal: ["Tajawal", "sans-serif"],
      },
      colors: {
        regular: {
          DEFAULT: "#0A0C11",
        },
        primary: {
          DEFAULT: "#00629B",
          med: "#166298",
          high: "#004F80",
        },
        secondary: {
          DEFAULT: "#5B616D",
          low: "#8C929C",
        },
        error: {
          DEFAULT: "#BF1722",
        },
      },
      letterSpacing: {
        extra1: "-0.05rem",
      },
      scale: {
        98: '0.98',
      },
    },
  },
  plugins: [
    twForms,
    plugin(function ({ addVariant }) {
      addVariant("a11y", ".a11y &");
    }),
  ],
};
