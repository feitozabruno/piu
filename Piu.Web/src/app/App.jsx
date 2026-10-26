import { useEffect } from "react";
import {
  BrowserRouter,
  Navigate,
  Route,
  Routes,
  useLocation,
  useNavigate,
} from "react-router-dom";
import { Home } from "@/features/lotes/Home";
import { TelaDeLogin } from "@/features/auth/TelaDeLogin";
import { RotaProtegida } from "@/features/auth/RotaProtegida";
import { MenuNavegacao } from "@/shared/components/MenuNavegacao";
import { PainelAdministracao } from "@/features/admin/PainelAdministracao";
import { PainelGerencia } from "@/features/admin/PainelGerencia";

function RedirecionarSessaoExpirada() {
  const location = useLocation();
  const navigate = useNavigate();

  useEffect(() => {
    function handleUnauthorized() {
      if (location.pathname !== "/entrar") {
        navigate("/entrar", {
          replace: true,
          state: { from: location },
        });
      }
    }

    window.addEventListener("auth:unauthorized", handleUnauthorized);

    return () => {
      window.removeEventListener("auth:unauthorized", handleUnauthorized);
    };
  }, [location, navigate]);

  return null;
}

function App() {
  return (
    <BrowserRouter>
      <RedirecionarSessaoExpirada />
      <MenuNavegacao />

      <Routes>
        <Route
          path="/"
          element={
            <RotaProtegida>
              <Home />
            </RotaProtegida>
          }
        />
        <Route path="/entrar" element={<TelaDeLogin />} />
        <Route
          path="/administracao"
          element={
            <RotaProtegida requiredProfile="Administrador">
              <PainelAdministracao />
            </RotaProtegida>
          }
        />
        <Route
          path="/gerencia"
          element={
            <RotaProtegida requiredProfile="Gerente">
              <PainelGerencia />
            </RotaProtegida>
          }
        />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
