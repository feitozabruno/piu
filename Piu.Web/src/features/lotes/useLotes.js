import { useCallback, useEffect, useState } from "react";
import { useLiveQuery } from "dexie-react-hooks";
import { apiFetch } from "@/shared/lib/api";
import { db, substituirLotes } from "@/infrastructure/offline/database";

export function useLotes(enabled) {
  const [error, setError] = useState(null);
  const lotes = useLiveQuery(() => db.lotes.orderBy("nome").toArray(), [], []);

  const carregarLotes = useCallback(async (estaAtivo = true) => {
    if (!navigator.onLine) return false;

    try {
      const lotesDaApi = await apiFetch("/lotes");
      if (!estaAtivo) return false;
      await substituirLotes(lotesDaApi);
      setError(null);
      return true;
    } catch (err) {
      if (!estaAtivo) return false;
      setError(
        err instanceof Error
          ? err.message
          : "Não foi possível carregar os lotes.",
      );
      return false;
    }
  }, []);

  useEffect(() => {
    if (!enabled) return;

    let ativo = true;

    async function sincronizar() {
      await carregarLotes(ativo);
    }

    void sincronizar();

    return () => {
      ativo = false;
    };
  }, [carregarLotes, enabled]);

  return { lotes, error, carregarLotes };
}
