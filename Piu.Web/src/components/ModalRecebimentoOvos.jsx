import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { CheckCircle2, ClipboardList } from "lucide-react";
import { Button } from "@/components/ui/button";
import { useReceberLote } from "@/hooks/useReceberLote";
import { useLotes } from "@/hooks/useLotes";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogFooter,
  DialogClose,
} from "@/components/ui/dialog";

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

const defaultValues = getDefaultValues();

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

export function ModalRecebimentoOvos({ open, onOpenChange }) {
  const [submitted, setSubmitted] = useState(false);
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
    defaultValues,
  });

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
      setSubmitted(true);
    }
  }

  return (
    <Dialog open={open} onOpenChange={handleOpen}>
      <DialogContent className="max-h-[90vh] overflow-y-auto rounded-2xl border-4 border-zinc-950 bg-white p-4 shadow-[0_7px_0_0_#18181b] sm:max-w-2xl sm:p-8">
        {submitted ? (
          <div aria-live="polite" className="flex flex-col items-start gap-3">
            <CheckCircle2
              className="size-14 text-emerald-700"
              aria-hidden="true"
            />
            <DialogHeader className="text-left">
              <DialogTitle className="text-3xl font-black uppercase tracking-tight">
                Recebimento registrado
              </DialogTitle>
              <DialogDescription className="text-base font-medium text-zinc-700">
                O recebimento foi salvo no dispositivo e será sincronizado
                automaticamente quando houver conexão.
              </DialogDescription>
            </DialogHeader>
            <DialogClose asChild>
              <Button
                type="button"
                className="mt-4 h-14 rounded-xl border-2 border-zinc-950 bg-[#f6c945] px-6 font-black uppercase text-zinc-950 shadow-[0_4px_0_0_#18181b] hover:bg-[#ffd95f]"
              >
                Fechar
              </Button>
            </DialogClose>
          </div>
        ) : (
          <form
            onSubmit={handleSubmit(handleReceiptSubmit)}
            noValidate
            className="flex flex-col gap-6"
          >
            <DialogHeader className="border-b-2 border-zinc-200 pb-5 text-left">
              <div className="flex items-center gap-3">
                <ClipboardList aria-hidden="true" className="size-8" />
                <DialogTitle className="text-xl font-black uppercase">
                  Dados do lote
                </DialogTitle>
              </div>
              <DialogDescription className="sr-only">
                Preencha os dados para registrar o recebimento de ovos.
              </DialogDescription>
            </DialogHeader>

            <div className="grid gap-5 sm:grid-cols-2">
              <FormField id="lote" label="Lote" error={errors.lote?.message}>
                <select
                  id="lote"
                  {...register("lote")}
                  aria-invalid={Boolean(errors.lote)}
                  className={fieldClass(Boolean(errors.lote))}
                >
                  <option value="">Selecione o lote</option>
                  {lotes.map((lote) => (
                    <option key={lote.id} value={lote.nome}>
                      {lote.nome}
                    </option>
                  ))}
                </select>
              </FormField>

              <FormField
                id="dataPostura"
                label="Data de postura"
                error={errors.dataPostura?.message}
              >
                <input
                  id="dataPostura"
                  type="date"
                  {...register("dataPostura")}
                  aria-invalid={Boolean(errors.dataPostura)}
                  className={fieldClass(Boolean(errors.dataPostura))}
                />
              </FormField>

              <FormField
                id="ninhoIncubavel"
                label="Quantidade Ovos Ninho"
                error={errors.ninhoIncubavel?.message}
              >
                <input
                  id="ninhoIncubavel"
                  type="number"
                  min="0"
                  step="1"
                  inputMode="numeric"
                  {...register("ninhoIncubavel")}
                  aria-invalid={Boolean(errors.ninhoIncubavel)}
                  className={fieldClass(Boolean(errors.ninhoIncubavel))}
                  placeholder="Ex.: 1200"
                />
              </FormField>

              <FormField
                id="camaIncubavel"
                label="Quantidade Ovos Cama"
                error={errors.camaIncubavel?.message}
              >
                <input
                  id="camaIncubavel"
                  type="number"
                  min="0"
                  step="1"
                  inputMode="numeric"
                  {...register("camaIncubavel")}
                  aria-invalid={Boolean(errors.camaIncubavel)}
                  className={fieldClass(Boolean(errors.camaIncubavel))}
                  placeholder="Ex.: 1200"
                />
              </FormField>

              <FormField
                id="temperaturaOvo"
                label="Temperatura Ovo"
                error={errors.temperaturaOvo?.message}
              >
                <input
                  id="temperaturaOvo"
                  type="number"
                  step="0.1"
                  {...register("temperaturaOvo")}
                  aria-invalid={Boolean(errors.temperaturaOvo)}
                  className={fieldClass(Boolean(errors.temperaturaOvo))}
                  placeholder="Ex.: 18.2"
                />
              </FormField>

              <FormField
                id="placaCaminhao"
                label="Placa Caminhão"
                error={errors.placaCaminhao?.message}
              >
                <input
                  id="placaCaminhao"
                  type="text"
                  {...register("placaCaminhao")}
                  aria-invalid={Boolean(errors.placaCaminhao)}
                  className={fieldClass(Boolean(errors.placaCaminhao))}
                  placeholder="Ex.: REW0H06"
                />
              </FormField>

              <FormField
                id="temperaturaBau"
                label="Temperatura Baú"
                error={errors.temperaturaBau?.message}
              >
                <input
                  id="temperaturaBau"
                  type="number"
                  step="0.1"
                  {...register("temperaturaBau")}
                  aria-invalid={Boolean(errors.temperaturaBau)}
                  className={fieldClass(Boolean(errors.temperaturaBau))}
                  placeholder="Ex.: 19.0"
                />
              </FormField>

              <FormField
                id="horaRecebimento"
                label="Hora Recebimento"
                error={errors.horaRecebimento?.message}
              >
                <input
                  id="horaRecebimento"
                  type="datetime-local"
                  {...register("horaRecebimento")}
                  aria-invalid={Boolean(errors.horaRecebimento)}
                  className={fieldClass(Boolean(errors.horaRecebimento))}
                />
              </FormField>

              <FormField
                id="motorista"
                label="Motorista"
                error={errors.motorista?.message}
              >
                <input
                  id="motorista"
                  type="text"
                  {...register("motorista")}
                  aria-invalid={Boolean(errors.motorista)}
                  className={fieldClass(Boolean(errors.motorista))}
                  placeholder="Ex.: VLADEMIR"
                />
              </FormField>

              <div className="sm:col-span-2">
                <FormField
                  id="observacao"
                  label="Observação"
                  error={errors.observacao?.message}
                >
                  <input
                    id="observacao"
                    type="text"
                    {...register("observacao")}
                    aria-invalid={Boolean(errors.observacao)}
                    className={fieldClass(Boolean(errors.observacao))}
                    placeholder="Ex.: Nenhuma anomalia."
                  />
                </FormField>
              </div>
            </div>

            {apiError && (
              <p className="text-sm font-bold text-red-700" role="alert">
                {apiError}
              </p>
            )}
            {lotesError && (
              <p className="text-sm font-bold text-red-700" role="alert">
                {lotesError}
              </p>
            )}

            <DialogFooter>
              <Button
                type="submit"
                disabled={isFormSubmitting || isSaving}
                className="h-14 w-full rounded-xl border-2 border-zinc-950 bg-[#f6c945] text-lg font-black uppercase text-zinc-950 shadow-[0_4px_0_0_#18181b] hover:bg-[#ffd95f]"
              >
                Registrar recebimento
              </Button>
            </DialogFooter>
          </form>
        )}
      </DialogContent>
    </Dialog>
  );
}

function FormField({ id, label, hint, error, children }) {
  return (
    <div className="flex flex-col gap-2">
      <label
        htmlFor={id}
        className="text-sm font-black uppercase tracking-wide"
      >
        {label}
      </label>
      {children}
      {hint && !error && (
        <p className="text-sm font-medium text-zinc-500">{hint}</p>
      )}
      {error && (
        <p className="text-sm font-bold text-red-700" role="alert">
          {error}
        </p>
      )}
    </div>
  );
}

function fieldClass(hasError) {
  return `h-14 w-full rounded-xl border-2 px-4 text-base font-bold outline-none transition focus:ring-4 focus:ring-[#f6c945]/50 ${
    hasError ? "border-red-700 bg-red-50" : "border-zinc-950 bg-white"
  }`;
}
