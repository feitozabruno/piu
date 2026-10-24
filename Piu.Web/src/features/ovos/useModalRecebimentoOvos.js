import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useReceberLote } from "@/features/ovos/useReceberLote";
import { useLotes } from "@/features/lotes/useLotes";

const MOTORISTAS_STORAGE_KEY = "piu.motoristas";
const PLACAS_STORAGE_KEY = "piu.placas-caminhao";

const receiptSchema = z
  .object({
    lote: z.string().trim().min(1, "Informe o lote."),
    dataPostura: z.string().min(1, "Informe a data de postura."),
    ninhoIncubavel: z.coerce
      .number({ message: "Informe a quantidade de ovos." })
      .int("Informe um número inteiro.")
      .min(0, "A quantidade não pode ser negativa."),
    camaIncubavel: z.coerce
      .number({ message: "Informe a quantidade de ovos." })
      .int("Informe um número inteiro.")
      .min(0, "A quantidade não pode ser negativa."),
    temperaturaOvo: z.coerce.number({ message: "Informe a temperatura." }),
    temperaturaBau: z.coerce.number({ message: "Informe a temperatura." }),
    placaCaminhao: z.string().trim().min(1, "Informe a placa do caminhão."),
    motorista: z.string().trim().min(1, "Informe o motorista."),
    horaRecebimento: z.string().min(1, "Informe a hora de recebimento."),
    observacao: z.string(),
  })
  .refine(
    ({ ninhoIncubavel, camaIncubavel }) =>
      ninhoIncubavel > 0 || camaIncubavel > 0,
    {
      message: "Informe uma quantidade de ovos maior que zero.",
      path: ["ninhoIncubavel"],
    },
  );

function getDefaultValues() {
  const agora = new Date();
  const ontem = new Date(agora);
  ontem.setDate(ontem.getDate() - 1);

  return {
    lote: "",
    dataPostura: formatarDataLocal(ontem),
    ninhoIncubavel: "",
    camaIncubavel: "",
    temperaturaOvo: "",
    temperaturaBau: "",
    placaCaminhao: "",
    motorista: "",
    horaRecebimento: formatarDataHoraLocal(agora),
    observacao: "",
  };
}

function getCachedValues(storageKey) {
  const savedValues = localStorage.getItem(storageKey);

  if (!savedValues) return [];

  try {
    const storedValues = JSON.parse(savedValues);

    return Array.isArray(storedValues)
      ? storedValues.filter(
          (value) => typeof value === "string" && value.trim(),
        )
      : [];
  } catch {
    localStorage.removeItem(storageKey);
    return [];
  }
}

function formatarDataLocal(data) {
  return [
    data.getFullYear(),
    String(data.getMonth() + 1).padStart(2, "0"),
    String(data.getDate()).padStart(2, "0"),
  ].join("-");
}

function formatarDataHoraLocal(data) {
  return `${formatarDataLocal(data)}T${String(data.getHours()).padStart(2, "0")}:${String(data.getMinutes()).padStart(2, "0")}`;
}

export function useModalRecebimentoOvos(open, onOpenChange) {
  const [submitted, setSubmitted] = useState(false);
  const [motoristas, setMotoristas] = useState(() =>
    getCachedValues(MOTORISTAS_STORAGE_KEY),
  );
  const [placas, setPlacas] = useState(() =>
    getCachedValues(PLACAS_STORAGE_KEY),
  );
  const {
    handleSubmit: submitReceipt,
    isSubmitting: isSaving,
    apiError,
  } = useReceberLote();
  const { lotes, error: lotesError } = useLotes(open);
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting: isFormSubmitting },
  } = useForm({
    resolver: zodResolver(receiptSchema),
    mode: "onChange",
    reValidateMode: "onChange",
    defaultValues: getDefaultValues(),
  });

  function registerUppercaseField(name) {
    const registration = register(name);

    return {
      ...registration,
      onChange: (event) => {
        event.target.value = event.target.value.toUpperCase();
        registration.onChange(event);
      },
    };
  }

  function handleOpen(nextOpen) {
    if (nextOpen) reset(getDefaultValues());
    handleOpenChange(nextOpen);
  }

  function handleOpenChange(nextOpen) {
    if (!nextOpen) {
      reset(getDefaultValues());
      setSubmitted(false);
    }
    onOpenChange(nextOpen);
  }

  async function handleReceiptSubmit(data) {
    if (await submitReceipt(data)) {
      updateCache(
        data.motorista,
        motoristas,
        setMotoristas,
        MOTORISTAS_STORAGE_KEY,
      );
      updateCache(data.placaCaminhao, placas, setPlacas, PLACAS_STORAGE_KEY);
      setSubmitted(true);
    }
  }

  return {
    apiError,
    errors,
    handleOpen,
    handleReceiptSubmit,
    handleSubmit,
    isFormSubmitting,
    isSaving,
    lotes,
    lotesError,
    motoristas,
    placas,
    register,
    registerUppercaseField,
    submitted,
  };
}

function updateCache(value, cachedValues, setCachedValues, storageKey) {
  const normalizedValue = value.trim().toUpperCase();
  const updatedValues = [
    normalizedValue,
    ...cachedValues.filter((item) => item !== normalizedValue),
  ];

  setCachedValues(updatedValues);
  localStorage.setItem(storageKey, JSON.stringify(updatedValues));
}
