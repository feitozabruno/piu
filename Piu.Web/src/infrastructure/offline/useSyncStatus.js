import { useEffect, useState } from "react";
import { useLiveQuery } from "dexie-react-hooks";
import { db } from "@/infrastructure/offline/database";

export function useSyncStatus() {
  const [online, setOnline] = useState(navigator.onLine);
  const resumo = useLiveQuery(async () => {
    const [recebimentos, configuracao] = await Promise.all([
      db.recebimentos.toArray(),
      db.configuracao.get("ultimaSincronizacao"),
    ]);

    return recebimentos.reduce(
      (resultado, recebimento) => {
        if (recebimento.status === "pending") resultado.pendentes += 1;
        if (recebimento.status === "syncing") resultado.sincronizando += 1;
        if (recebimento.status === "failed") resultado.falhas += 1;
        return resultado;
      },
      {
        pendentes: 0,
        sincronizando: 0,
        falhas: 0,
        ultimaSincronizacao: configuracao?.valor ?? null,
      },
    );
  }, []);

  useEffect(() => {
    const ficarOnline = () => setOnline(true);
    const ficarOffline = () => setOnline(false);

    window.addEventListener("online", ficarOnline);
    window.addEventListener("offline", ficarOffline);

    return () => {
      window.removeEventListener("online", ficarOnline);
      window.removeEventListener("offline", ficarOffline);
    };
  }, []);

  return {
    online,
    ...(resumo ?? {
      pendentes: 0,
      sincronizando: 0,
      falhas: 0,
      ultimaSincronizacao: null,
    }),
  };
}
