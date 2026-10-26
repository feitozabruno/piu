import { useEffect, useState } from "react";
import { apiFetch } from "@/shared/lib/api";

export function useAuth(pathname) {
  const [isAuthenticated, setIsAuthenticated] = useState(null);
  const [error, setError] = useState(null);

  useEffect(() => {
    let isMounted = true;

    apiFetch("/auth/sessao")
      .then((session) => {
        if (!isMounted) return;

        setIsAuthenticated(session?.autenticado === true);
        setError(null);
      })
      .catch((err) => {
        if (!isMounted) return;

        setIsAuthenticated(false);
        setError(err.message);
      });

    return () => {
      isMounted = false;
    };
  }, [pathname]);

  const logout = async () => {
    setError(null);

    try {
      await apiFetch("/auth/sair", { method: "POST" });
      setIsAuthenticated(false);
      return true;
    } catch (err) {
      setError(err.message);
      return false;
    }
  };

  return { isAuthenticated, error, logout };
}
