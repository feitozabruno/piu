import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "@/features/auth/useAuth";

export function RotaProtegida({ children, requiredProfile }) {
  const location = useLocation();
  const { isAuthenticated, perfil } = useAuth(location.pathname);

  if (isAuthenticated === null) {
    return null;
  }

  if (!isAuthenticated) {
    return <Navigate to="/entrar" replace state={{ from: location }} />;
  }

  if (requiredProfile && perfil !== requiredProfile) {
    return <Navigate to="/" replace />;
  }

  return children;
}
