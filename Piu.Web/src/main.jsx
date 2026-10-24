import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { registerSW } from "virtual:pwa-register";
import { iniciarSincronizacao } from "@/infrastructure/offline/sync";
import "./App.css";
import App from "./app/App";

registerSW({ immediate: true });
iniciarSincronizacao();

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <App />
  </StrictMode>,
);
