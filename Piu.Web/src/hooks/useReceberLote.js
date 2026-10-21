import { useState } from "react";
import { enfileirarRecebimento } from "@/lib/database";
import { sincronizarRecebimentos } from "@/lib/sync";

export function useReceberLote() {
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [apiError, setApiError] = useState(null);

  const handleSubmit = async (formData) => {
    setIsSubmitting(true);
    setApiError(null);

    try {
      const horaRecebimento =
        formData.horaRecebimento.length === 16
          ? `${formData.horaRecebimento}:00`
          : formData.horaRecebimento;

      await enfileirarRecebimento({ ...formData, horaRecebimento });
      void sincronizarRecebimentos();
      return true;
    } catch (err) {
      setApiError(
        err instanceof Error ? err.message : "Erro ao registrar recebimento.",
      );
      return false;
    } finally {
      setIsSubmitting(false);
    }
  };

  return {
    handleSubmit,
    isSubmitting,
    apiError,
  };
}
