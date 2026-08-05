import twForms from "@tailwindcss/forms";
import plugin from "tailwindcss/plugin";

/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  safelist: ["md:w-[27%]", "md:w-[43%]"],
  darkMode: "class",
  theme: {
    extend: {
      fontFamily: {
        sans: ["Arial", "sans-serif"],
        tajawal: ["Tajawal", "sans-serif"],
      },
      colors: {
        regular: {
          DEFAULT: "rgb(var(--color-regular) / <alpha-value>)",
        },
        primary: {
          DEFAULT: "rgb(var(--color-primary) / <alpha-value>)",
          med: "rgb(var(--color-primary-med) / <alpha-value>)",
          high: "rgb(var(--color-primary-high) / <alpha-value>)",
        },
        secondary: {
          DEFAULT: "rgb(var(--color-secondary) / <alpha-value>)",
          low: "rgb(var(--color-secondary-low) / <alpha-value>)",
        },
        error: {
          DEFAULT: "rgb(var(--color-error) / <alpha-value>)",
        },
        on: {
          primary: "rgb(var(--color-on-primary) / <alpha-value>)",
        },
        surface: {
          DEFAULT: "rgb(var(--color-surface) / <alpha-value>)",
        },
        raised: {
          DEFAULT: "rgb(var(--color-raised) / <alpha-value>)",
        },
        hover: {
          DEFAULT: "rgb(var(--color-hover) / <alpha-value>)",
        },
        disabled: {
          bg: "rgb(var(--color-disabled-bg) / <alpha-value>)",
          fg: "rgb(var(--color-disabled-fg) / <alpha-value>)",
        },
        line: {
          DEFAULT: "rgb(var(--color-line) / <alpha-value>)",
          strong: "rgb(var(--color-line-strong) / <alpha-value>)",
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
