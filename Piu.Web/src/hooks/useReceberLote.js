import { useState } from "react";

const BASE_URL = "https://piu-vhfi.onrender.com/api/v1";

export async function apiFetch(endpoint, options = {}) {
  const response = await fetch(`${BASE_URL}${endpoint}`, {
    ...options,
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
      ...options.headers,
    },
  });

  if (!response.ok) {
    const errorData = await response.json().catch(() => null);
    const message =
      errorData?.detail ??
      errorData?.title ??
      errorData?.message ??
      "Erro na requisição";
    throw new Error(message);
  }

  if (response.status === 204) return null;

  return response.json();
}

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

      await apiFetch("/ovos/", {
        method: "POST",
        body: JSON.stringify({ ...formData, horaRecebimento }),
      });
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
