import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "@/features/auth/useAuth";

export function RotaProtegida({ children }) {
  const location = useLocation();
  const { isAuthenticated } = useAuth(location.pathname);

  if (isAuthenticated === null) {
    return null;
  }

  if (!isAuthenticated) {
    return <Navigate to="/entrar" replace state={{ from: location }} />;
  }

  return children;
}
