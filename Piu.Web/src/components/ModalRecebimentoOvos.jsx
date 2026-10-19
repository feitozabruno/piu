import { useState } from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { CheckCircle2, ClipboardList } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogFooter,
  DialogClose,
} from "@/components/ui/dialog";

const receiptSchema = z.object({
  originFarm: z.string().min(1, "Selecione a granja de origem."),
  layingDate: z.string().min(1, "Informe a data de postura."),
  eggQuantity: z.coerce
    .number({ message: "Informe a quantidade de ovos." })
    .positive("A quantidade deve ser maior que zero."),
  batchInvoice: z.string().trim().min(1, "Informe o lote ou a NF."),
});

const defaultValues = {
  originFarm: "",
  layingDate: "",
  eggQuantity: "",
  batchInvoice: "",
};

export function ModalRecebimentoOvos({ open, onOpenChange }) {
  const [submitted, setSubmitted] = useState(false);
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting },
  } = useForm({
    resolver: zodResolver(receiptSchema),
    mode: "onChange",
    reValidateMode: "onChange",
    defaultValues,
  });

  function handleOpenChange(nextOpen) {
    if (!nextOpen) {
      reset(defaultValues);
      setSubmitted(false);
    }
    onOpenChange(nextOpen);
  }

  return (
    <Dialog open={open} onOpenChange={handleOpenChange}>
      <DialogContent className="max-h-[90vh] overflow-y-auto rounded-2xl border-4 border-zinc-950 bg-white p-6 shadow-[0_7px_0_0_#18181b] sm:max-w-xl sm:p-8">
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
                O novo lote foi validado e está pronto para sincronização.
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
            onSubmit={handleSubmit(() => setSubmitted(true))}
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

            <FormField
              id="batchInvoice"
              label="Lote"
              error={errors.batchInvoice?.message}
            >
              <input
                id="batchInvoice"
                type="text"
                {...register("batchInvoice")}
                aria-invalid={Boolean(errors.batchInvoice)}
                className={fieldClass(Boolean(errors.batchInvoice))}
                placeholder="Ex.: DRD03-012"
              />
            </FormField>

            <FormField
              id="layingDate"
              label="Data de postura"
              error={errors.layingDate?.message}
            >
              <input
                id="layingDate"
                type="date"
                {...register("layingDate")}
                aria-invalid={Boolean(errors.layingDate)}
                className={fieldClass(Boolean(errors.layingDate))}
              />
            </FormField>

            <FormField
              id="eggQuantity"
              label="Quantidade Ovos Ninho"
              error={errors.eggQuantity?.message}
            >
              <input
                id="eggQuantity"
                type="number"
                min="1"
                step="1"
                inputMode="numeric"
                {...register("eggQuantity")}
                aria-invalid={Boolean(errors.eggQuantity)}
                className={fieldClass(Boolean(errors.eggQuantity))}
                placeholder="Ex.: 1200"
              />
            </FormField>

            <FormField
              id="ovosCama"
              label="Quantidade Ovos Cama"
              error={errors.eggQuantity?.message}
            >
              <input
                id="eggQuantity"
                type="number"
                min="1"
                step="1"
                inputMode="numeric"
                {...register("eggQuantity")}
                aria-invalid={Boolean(errors.eggQuantity)}
                className={fieldClass(Boolean(errors.eggQuantity))}
                placeholder="Ex.: 1200"
              />
            </FormField>

            <FormField
              id="temperaturaOvo"
              label="Temperatura Ovo"
              error={errors.batchInvoice?.message}
            >
              <input
                id="placa"
                type="text"
                {...register("temperaturaOvo")}
                aria-invalid={Boolean(errors.batchInvoice)}
                className={fieldClass(Boolean(errors.batchInvoice))}
                placeholder="Ex.: 18.2"
              />
            </FormField>

            <FormField
              id="placa"
              label="Placa Caminhão"
              error={errors.batchInvoice?.message}
            >
              <input
                id="placa"
                type="text"
                {...register("placa")}
                aria-invalid={Boolean(errors.batchInvoice)}
                className={fieldClass(Boolean(errors.batchInvoice))}
                placeholder="Ex.: REW0H06"
              />
            </FormField>

            <FormField
              id="temperaturaBau"
              label="Temperatura Baú"
              error={errors.batchInvoice?.message}
            >
              <input
                id="placa"
                type="text"
                {...register("temperaturaBau")}
                aria-invalid={Boolean(errors.batchInvoice)}
                className={fieldClass(Boolean(errors.batchInvoice))}
                placeholder="Ex.: 19.0"
              />
            </FormField>

            <FormField
              id="horaRecebimento"
              label="Hora Recebimento"
              error={errors.batchInvoice?.message}
            >
              <input
                id="horaRecebimento"
                type="text"
                {...register("horaRecebimento")}
                aria-invalid={Boolean(errors.batchInvoice)}
                className={fieldClass(Boolean(errors.batchInvoice))}
                placeholder="Ex.: 09:30"
              />
            </FormField>

            <FormField
              id="motorista"
              label="Motorista"
              error={errors.batchInvoice?.message}
            >
              <input
                id="placa"
                type="text"
                {...register("motorista")}
                aria-invalid={Boolean(errors.batchInvoice)}
                className={fieldClass(Boolean(errors.batchInvoice))}
                placeholder="Ex.: VLADEMIR"
              />
            </FormField>

            <FormField
              id="observacao"
              label="Observação"
              error={errors.batchInvoice?.message}
            >
              <input
                id="observacao"
                type="text"
                {...register("observacao")}
                aria-invalid={Boolean(errors.batchInvoice)}
                className={fieldClass(Boolean(errors.batchInvoice))}
                placeholder="Ex.: Nenhuma anomalia."
              />
            </FormField>

            <DialogFooter>
              <Button
                type="submit"
                disabled={isSubmitting}
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
