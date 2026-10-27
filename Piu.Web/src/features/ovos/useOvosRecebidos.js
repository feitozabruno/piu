import { useCallback, useEffect, useState } from "react";
import { apiFetch } from "@/shared/lib/api";

export function useOvosRecebidos(dataInicial = "", dataFinal = "") {
  const [ovos, setOvos] = useState([]);
  const [error, setError] = useState(null);
  const [carregando, setCarregando] = useState(true);

  const carregarOvos = useCallback(async (dataInicial = "", dataFinal = "") => {
    setCarregando(true);

    try {
      const params = new URLSearchParams();
      if (dataInicial) params.set("dataInicial", dataInicial);
      if (dataFinal) params.set("dataFinal", dataFinal);

      const resultado = await apiFetch(
        `/ovos${params.size > 0 ? `?${params}` : ""}`,
      );
      setOvos(Array.isArray(resultado) ? resultado : []);
      setError(null);
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Não foi possível carregar os ovos recebidos.",
      );
    } finally {
      setCarregando(false);
    }
  }, []);

  useEffect(() => {
    async function sincronizar() {
      await carregarOvos(dataInicial, dataFinal);
    }

    void sincronizar();
  }, [carregarOvos, dataInicial, dataFinal]);

  return { ovos, error, carregando, carregarOvos };
}
