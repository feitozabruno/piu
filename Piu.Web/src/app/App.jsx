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
import { Relatorios } from "@/features/relatorios/Relatorios";
import { RotaProtegida } from "@/features/auth/RotaProtegida";
import { MenuNavegacao } from "@/shared/components/MenuNavegacao";

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
        <Route
          path="/relatorios"
          element={
            <RotaProtegida>
              <Relatorios />
            </RotaProtegida>
          }
        />
        <Route path="/entrar" element={<TelaDeLogin />} />
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
