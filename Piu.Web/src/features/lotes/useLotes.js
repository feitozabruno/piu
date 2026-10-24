import { useEffect, useState } from "react";
import { useLiveQuery } from "dexie-react-hooks";
import { apiFetch } from "@/shared/lib/api";
import { db, substituirLotes } from "@/infrastructure/offline/database";

export function useLotes(enabled) {
  const [error, setError] = useState(null);
  const lotes = useLiveQuery(() => db.lotes.orderBy("nome").toArray(), [], []);

  useEffect(() => {
    if (!enabled || !navigator.onLine) return;

    let ativo = true;

    async function carregarLotes() {
      try {
        const lotesDaApi = await apiFetch("/lotes");
        await substituirLotes(lotesDaApi);
      } catch (err) {
        if (ativo) {
          setError(
            err instanceof Error
              ? err.message
              : "Não foi possível carregar os lotes.",
          );
        }
      }
    }

    void carregarLotes();

    return () => {
      ativo = false;
    };
  }, [enabled]);

  return { lotes, error };
}
