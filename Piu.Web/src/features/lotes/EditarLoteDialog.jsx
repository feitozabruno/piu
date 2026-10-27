import { CheckCircle2, LoaderCircle, Pencil, ToggleLeft } from "lucide-react";
import { Button } from "@/shared/components/ui/button";
import { Input } from "@/shared/components/ui/input";
import {
  Dialog,
  DialogClose,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/shared/components/ui/dialog";

export function EditarLoteDialog({ editor }) {
  return (
    <Dialog
      open={editor.aberto}
      onOpenChange={(open) => {
        if (!open) editor.fechar();
      }}
    >
      <DialogContent className="max-w-md overflow-hidden rounded-[28px] border-2 border-zinc-950 bg-white p-0 shadow-[8px_8px_0_0_#18181b]">
        <form onSubmit={editor.submeter} noValidate>
          <div className="p-6 sm:p-8">
            <DialogHeader className="text-left">
              <div className="mb-5 flex items-center gap-3">
                <div className="flex size-12 items-center justify-center rounded-2xl border-2 border-zinc-950 bg-[#f6c945]">
                  <Pencil
                    className="size-5"
                    strokeWidth={3}
                    aria-hidden="true"
                  />
                </div>
                <div>
                  <DialogTitle className="text-2xl font-black uppercase tracking-tight">
                    Editar lote
                  </DialogTitle>
                  <DialogDescription className="mt-1 text-sm font-medium text-zinc-600">
                    Mantenha os dados do lote atualizados.
                  </DialogDescription>
                </div>
              </div>
              <div className="h-1 rounded-full bg-[#f6c945]" />
            </DialogHeader>

            <div className="grid gap-5 pt-6">
              <Field label="Nome do lote" htmlFor="nome-lote-edicao">
                <Input
                  id="nome-lote-edicao"
                  value={editor.nome}
                  onChange={(event) => editor.setNome(event.target.value)}
                  maxLength={30}
                  placeholder="Ex.: LOTE 01"
                  disabled={editor.salvando}
                  aria-invalid={Boolean(editor.erro)}
                  className="h-12 rounded-xl border-2 border-zinc-300"
                />
              </Field>
              <Field
                label="Data de nascimento"
                htmlFor="data-nascimento-lote-edicao"
              >
                <Input
                  id="data-nascimento-lote-edicao"
                  type="date"
                  value={editor.dataNascimento}
                  onChange={(event) =>
                    editor.setDataNascimento(event.target.value)
                  }
                  max={new Date().toISOString().slice(0, 10)}
                  disabled={editor.salvando}
                  aria-invalid={Boolean(editor.erro)}
                  className="h-12 rounded-xl border-2 border-zinc-300"
                />
              </Field>

              <button
                type="button"
                onClick={() => editor.setAtivo(!editor.ativo)}
                disabled={editor.salvando}
                className={`flex items-center justify-between rounded-2xl border-2 p-4 text-left transition-colors ${
                  editor.ativo
                    ? "border-emerald-700 bg-emerald-50"
                    : "border-zinc-300 bg-zinc-50"
                }`}
              >
                <span className="flex items-center gap-3">
                  <ToggleLeft
                    className={`size-6 ${
                      editor.ativo ? "text-emerald-700" : "text-zinc-500"
                    }`}
                    aria-hidden="true"
                  />
                  <span>
                    <span className="block text-sm font-black uppercase">
                      Status do lote
                    </span>
                    <span className="block text-sm font-medium text-zinc-600">
                      {editor.ativo
                        ? "Disponível para operação"
                        : "Indisponível para operação"}
                    </span>
                  </span>
                </span>
                <span
                  className={`rounded-full px-3 py-1 text-xs font-black uppercase ${
                    editor.ativo
                      ? "bg-emerald-200 text-emerald-900"
                      : "bg-zinc-200 text-zinc-700"
                  }`}
                >
                  {editor.ativo ? "Ativo" : "Inativo"}
                </span>
              </button>

              {editor.erro && (
                <p
                  className="rounded-xl border-2 border-red-700 bg-red-50 px-3 py-2 text-sm font-bold text-red-800"
                  role="alert"
                >
                  {editor.erro}
                </p>
              )}
            </div>
          </div>

          <DialogFooter className="border-t-2 border-zinc-200 bg-zinc-50 p-5 sm:p-6">
            <DialogClose
              render={<Button type="button" variant="outline" />}
              disabled={editor.salvando}
              className="h-11 rounded-xl border-2 border-zinc-300 font-black uppercase"
            >
              Cancelar
            </DialogClose>
            <Button
              type="submit"
              disabled={editor.salvando}
              className="h-11 rounded-xl border-2 border-zinc-950 bg-[#f6c945] font-black uppercase text-zinc-950 shadow-[0_3px_0_0_#18181b] hover:bg-[#ffd95f] active:translate-y-0.5 active:shadow-none"
            >
              {editor.salvando ? (
                <LoaderCircle
                  className="size-4 animate-spin"
                  aria-hidden="true"
                />
              ) : (
                <CheckCircle2 className="size-4" aria-hidden="true" />
              )}
              {editor.salvando ? "Salvando..." : "Salvar alterações"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}

function Field({ children, htmlFor, label }) {
  return (
    <label
      className="grid gap-2 text-sm font-black uppercase tracking-wide"
      htmlFor={htmlFor}
    >
      {label}
      {children}
    </label>
  );
}
