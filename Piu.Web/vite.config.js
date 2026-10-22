import path from "path";
import { fileURLToPath } from "url";
import react from "@vitejs/plugin-react";
import { defineConfig } from "vite";
import tailwindcss from "@tailwindcss/vite";
import { VitePWA } from "vite-plugin-pwa";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

export default defineConfig(({ mode }) => ({
  server: {
    proxy: {
      "/api": {
        target:
          mode === "development"
            ? "http://localhost:5000"
            : "https://piu-vhfi.onrender.com",
        changeOrigin: true,
        secure: true,
      },
    },
  },
  plugins: [
    react(),
    tailwindcss(),
    VitePWA({
      registerType: "autoUpdate",
      includeAssets: ["favicon.svg"],
      manifest: {
        name: "Piu",
        short_name: "Piu",
        description: "Gestão de incubatórios e recebimento de ovos.",
        lang: "pt-BR",
        start_url: "/",
        scope: "/",
        display: "standalone",
        theme_color: "#ff5a00",
        background_color: "#ffffff",
        icons: [
          {
            src: "/pwa-192x192.svg",
            sizes: "192x192",
            type: "image/svg+xml",
            purpose: "any",
          },
          {
            src: "/pwa-512x512.svg",
            sizes: "512x512",
            type: "image/svg+xml",
            purpose: "any maskable",
          },
        ],
      },
      workbox: {
        cleanupOutdatedCaches: true,
        navigateFallback: "/index.html",
        globPatterns: ["**/*.{js,css,html,ico,png,svg,woff2}"],
      },
    }),
  ],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
    },
  },
  check: {
    lint: true,
    format: true,
  },
}));
