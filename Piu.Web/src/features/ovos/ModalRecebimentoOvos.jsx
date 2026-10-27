import { CheckCircle2, ClipboardList } from "lucide-react";
import { Button } from "@/shared/components/ui/button";
import { Input } from "@/shared/components/ui/input";
import { Label } from "@/shared/components/ui/label";
import { Select } from "@/shared/components/ui/select";
import { useModalRecebimentoOvos } from "@/features/ovos/useModalRecebimentoOvos";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogClose,
} from "@/shared/components/ui/dialog";

export function ModalRecebimentoOvos({ open, onOpenChange }) {
  const {
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
  } = useModalRecebimentoOvos(open, onOpenChange);

  return (
    <Dialog open={open} onOpenChange={handleOpen}>
      <DialogContent className="overflow-hidden rounded-2xl border-4 border-zinc-950 bg-white p-0 shadow-[0_7px_0_0_#18181b] sm:max-w-2xl">
        <div className="m-1 max-h-[calc(90vh-0.5rem)] overflow-y-auto p-3 sm:m-2 sm:max-h-[calc(90vh-1rem)] sm:p-6">
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
                  <Select
                    id="lote"
                    {...register("lote")}
                    aria-invalid={Boolean(errors.lote)}
                    className={`${fieldClass(Boolean(errors.lote))} uppercase`}
                  >
                    <option value="">Selecione o lote</option>
                    {lotes.map((lote) => (
                      <option key={lote.id} value={lote.nome.toUpperCase()}>
                        {lote.nome.toUpperCase()}
                      </option>
                    ))}
                  </Select>
                </FormField>

                <FormField
                  id="dataPostura"
                  label="Data de postura"
                  error={errors.dataPostura?.message}
                >
                  <Input
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
                  <Input
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
                  <Input
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
                  <Input
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
                  <Input
                    id="placaCaminhao"
                    type="text"
                    {...registerUppercaseField("placaCaminhao")}
                    list="placas-disponiveis"
                    aria-invalid={Boolean(errors.placaCaminhao)}
                    className={`${fieldClass(Boolean(errors.placaCaminhao))} uppercase`}
                    placeholder="Ex.: REW0H06"
                  />
                  <datalist id="placas-disponiveis">
                    {placas.map((placa) => (
                      <option key={placa} value={placa} />
                    ))}
                  </datalist>
                </FormField>

                <FormField
                  id="temperaturaBau"
                  label="Temperatura Baú"
                  error={errors.temperaturaBau?.message}
                >
                  <Input
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
                  <Input
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
                  <Input
                    id="motorista"
                    type="text"
                    {...registerUppercaseField("motorista")}
                    list="motoristas-disponiveis"
                    aria-invalid={Boolean(errors.motorista)}
                    className={`${fieldClass(Boolean(errors.motorista))} uppercase`}
                    placeholder="Ex.: VLADEMIR"
                  />
                  <datalist id="motoristas-disponiveis">
                    {motoristas.map((motorista) => (
                      <option key={motorista} value={motorista} />
                    ))}
                  </datalist>
                </FormField>

                <div className="sm:col-span-2">
                  <FormField
                    id="observacao"
                    label="Observação"
                    error={errors.observacao?.message}
                  >
                    <Input
                      id="observacao"
                      type="text"
                      {...registerUppercaseField("observacao")}
                      aria-invalid={Boolean(errors.observacao)}
                      className={`${fieldClass(Boolean(errors.observacao))} uppercase`}
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

              <Button
                type="submit"
                disabled={isFormSubmitting || isSaving}
                className="h-14 w-full rounded-xl border-2 border-zinc-950 bg-[#f6c945] text-lg font-black uppercase text-zinc-950 shadow-[0_4px_0_0_#18181b] hover:bg-[#ffd95f]"
              >
                Registrar recebimento
              </Button>
            </form>
          )}
        </div>
      </DialogContent>
    </Dialog>
  );
}

function FormField({ id, label, hint, error, children }) {
  return (
    <div className="flex flex-col gap-2">
      <Label
        htmlFor={id}
        className="text-sm font-black uppercase tracking-wide"
      >
        {label}
      </Label>
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
