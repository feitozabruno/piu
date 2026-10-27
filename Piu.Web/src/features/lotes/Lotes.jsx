import { useMemo, useState } from "react";
import {
  CalendarDays,
  CheckCircle2,
  ClipboardList,
  LoaderCircle,
  Pencil,
  Plus,
  RefreshCw,
  Search,
  WifiOff,
} from "lucide-react";
import { apiFetch } from "@/shared/lib/api";
import { Button } from "@/shared/components/ui/button";
import { Input } from "@/shared/components/ui/input";
import { useLotes } from "@/features/lotes/useLotes";
import { EditarLoteDialog } from "@/features/lotes/EditarLoteDialog";
import { useEditarLote } from "@/features/lotes/useEditarLote";

export function Lotes() {
  const { lotes, error, carregarLotes } = useLotes(true);
  const [busca, setBusca] = useState("");
  const [nome, setNome] = useState("");
  const [dataNascimento, setDataNascimento] = useState("");
  const [formError, setFormError] = useState(null);
  const [salvando, setSalvando] = useState(false);
  const [salvo, setSalvo] = useState(false);
  const [atualizando, setAtualizando] = useState(false);
  const editor = useEditarLote(carregarLotes);

  const lotesFiltrados = useMemo(() => {
    const termo = busca.trim().toLocaleLowerCase();
    if (!termo) return lotes;

    return lotes.filter((lote) =>
      lote.nome.toLocaleLowerCase().includes(termo),
    );
  }, [busca, lotes]);

  async function handleSubmit(event) {
    event.preventDefault();
    setSalvo(false);
    setFormError(null);

    const nomeNormalizado = nome.trim();
    if (nomeNormalizado.length < 3 || nomeNormalizado.length > 30) {
      setFormError("O nome deve ter entre 3 e 30 caracteres.");
      return;
    }

    if (!dataNascimento) {
      setFormError("Informe a data de nascimento.");
      return;
    }

    if (dataNascimento > new Date().toISOString().slice(0, 10)) {
      setFormError("A data de nascimento não pode estar no futuro.");
      return;
    }

    setSalvando(true);

    try {
      await apiFetch("/lotes", {
        method: "POST",
        body: JSON.stringify({
          nome: nomeNormalizado,
          dataNascimento,
        }),
      });
      setNome("");
      setDataNascimento("");
      setSalvo(true);
      await carregarLotes();
    } catch (err) {
      setFormError(
        err instanceof Error
          ? err.message
          : "Não foi possível cadastrar o lote.",
      );
    } finally {
      setSalvando(false);
    }
  }

  async function atualizarLista() {
    setAtualizando(true);
    await carregarLotes();
    setAtualizando(false);
  }

  return (
    <main className="min-h-[calc(100vh-73px)] bg-[#f4f6f8] text-zinc-950">
      <div className="mx-auto max-w-7xl px-4 py-6 sm:px-8 sm:py-10 lg:px-10">
        <header className="mb-8 flex flex-col gap-5 sm:mb-10 sm:flex-row sm:items-end sm:justify-between">
          <div>
            <p className="mb-2 text-xs font-black uppercase tracking-[0.18em] text-zinc-500">
              Cadastro e operação
            </p>
            <h1 className="text-3xl font-black uppercase leading-[0.95] tracking-tight sm:text-5xl">
              Gerenciar lotes
            </h1>
            <p className="mt-3 max-w-2xl text-base font-medium leading-snug text-zinc-600 sm:text-xl">
              Cadastre e consulte os lotes disponíveis para a operação.
            </p>
          </div>
          <div className="flex items-center gap-2 self-start rounded-full border-2 border-zinc-950 bg-white px-4 py-2 text-xs font-black uppercase tracking-[0.12em] sm:self-auto">
            <ClipboardList className="size-4" aria-hidden="true" />
            {lotes.length} {lotes.length === 1 ? "lote" : "lotes"}
          </div>
        </header>

        <div className="grid gap-6 lg:grid-cols-[minmax(280px,0.8fr)_minmax(0,1.4fr)]">
          <section className="h-fit rounded-[24px] border-2 border-zinc-950 bg-white p-5 shadow-[6px_6px_0_0_#18181b] sm:p-7">
            <div className="mb-6 flex items-start gap-3">
              <div className="flex size-12 shrink-0 items-center justify-center rounded-xl border-2 border-zinc-950 bg-[#f6c945]">
                <Plus className="size-6" strokeWidth={3} aria-hidden="true" />
              </div>
              <div>
                <h2 className="text-xl font-black uppercase">Novo lote</h2>
                <p className="mt-1 text-sm font-medium text-zinc-600">
                  Informe os dados para disponibilizar o lote.
                </p>
              </div>
            </div>

            <form onSubmit={handleSubmit} noValidate className="grid gap-5">
              <label
                className="grid gap-2 text-sm font-black uppercase tracking-wide"
                htmlFor="nome-lote"
              >
                Nome do lote
                <Input
                  id="nome-lote"
                  value={nome}
                  onChange={(event) => setNome(event.target.value)}
                  maxLength={30}
                  placeholder="Ex.: LOTE 01"
                  disabled={salvando}
                  aria-invalid={Boolean(formError)}
                />
              </label>
              <label
                className="grid gap-2 text-sm font-black uppercase tracking-wide"
                htmlFor="data-nascimento-lote"
              >
                Data de nascimento
                <Input
                  id="data-nascimento-lote"
                  type="date"
                  value={dataNascimento}
                  onChange={(event) => setDataNascimento(event.target.value)}
                  max={new Date().toISOString().slice(0, 10)}
                  disabled={salvando}
                  aria-invalid={Boolean(formError)}
                />
              </label>

              {formError && (
                <p
                  className="rounded-xl border-2 border-red-700 bg-red-50 px-3 py-2 text-sm font-bold text-red-800"
                  role="alert"
                >
                  {formError}
                </p>
              )}
              {salvo && (
                <p
                  className="flex items-center gap-2 rounded-xl border-2 border-emerald-700 bg-emerald-50 px-3 py-2 text-sm font-bold text-emerald-800"
                  role="status"
                >
                  <CheckCircle2
                    className="size-4 shrink-0"
                    aria-hidden="true"
                  />
                  Lote cadastrado com sucesso.
                </p>
              )}

              <Button
                type="submit"
                disabled={salvando}
                className="h-12 rounded-xl border-2 border-zinc-950 bg-[#f6c945] font-black uppercase text-zinc-950 shadow-[0_4px_0_0_#18181b] hover:bg-[#ffd95f] active:translate-y-1 active:shadow-none"
              >
                {salvando ? (
                  <LoaderCircle
                    className="size-5 animate-spin"
                    aria-hidden="true"
                  />
                ) : (
                  <Plus className="size-5" aria-hidden="true" />
                )}
                {salvando ? "Salvando..." : "Cadastrar lote"}
              </Button>
            </form>
          </section>

          <section className="rounded-[24px] border-2 border-zinc-950 bg-white p-5 sm:p-7">
            <div className="mb-6 flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
              <div>
                <h2 className="text-xl font-black uppercase">
                  Lotes cadastrados
                </h2>
                <p className="mt-1 text-sm font-medium text-zinc-600">
                  Lista atualizada por último na API quando houver conexão.
                </p>
              </div>
              <Button
                type="button"
                variant="outline"
                onClick={atualizarLista}
                disabled={atualizando || !navigator.onLine}
                className="h-10 rounded-xl border-2 border-zinc-950 px-3 font-black uppercase"
              >
                <RefreshCw
                  className={`size-4 ${atualizando ? "animate-spin" : ""}`}
                  aria-hidden="true"
                />
                Atualizar
              </Button>
            </div>

            <div className="relative mb-5">
              <Search
                className="pointer-events-none absolute left-3 top-1/2 size-5 -translate-y-1/2 text-zinc-500"
                aria-hidden="true"
              />
              <Input
                value={busca}
                onChange={(event) => setBusca(event.target.value)}
                placeholder="Buscar por nome"
                aria-label="Buscar lote por nome"
                className="h-12 rounded-xl border-2 border-zinc-300 pl-10"
              />
            </div>

            {error && (
              <p
                className="mb-5 rounded-xl border-2 border-red-700 bg-red-50 px-3 py-2 text-sm font-bold text-red-800"
                role="alert"
              >
                {error}
              </p>
            )}
            {!navigator.onLine && (
              <p className="mb-5 flex items-center gap-2 rounded-xl border-2 border-zinc-300 bg-zinc-100 px-3 py-2 text-sm font-bold text-zinc-700">
                <WifiOff className="size-4" aria-hidden="true" />
                Exibindo os lotes salvos neste dispositivo.
              </p>
            )}

            {lotesFiltrados.length > 0 ? (
              <div
                className="grid gap-3"
                role="list"
                aria-label="Lotes cadastrados"
              >
                {lotesFiltrados.map((lote) => (
                  <article
                    key={lote.id}
                    role="listitem"
                    className="flex items-center justify-between gap-4 rounded-xl border-2 border-zinc-200 p-4"
                  >
                    <div className="min-w-0">
                      <p className="truncate text-lg font-black uppercase">
                        {lote.nome}
                      </p>
                      <p className="mt-1 flex items-center gap-1.5 text-sm font-bold text-zinc-500">
                        <CalendarDays className="size-4" aria-hidden="true" />
                        Nascimento: {formatarData(lote.dataNascimento)}
                      </p>
                    </div>
                    <div className="flex shrink-0 items-center gap-2">
                      <span
                        className={`rounded-full px-3 py-1 text-xs font-black uppercase ${
                          lote.ativo
                            ? "bg-emerald-100 text-emerald-800"
                            : "bg-zinc-200 text-zinc-700"
                        }`}
                      >
                        {lote.ativo ? "Ativo" : "Inativo"}
                      </span>
                      <Button
                        type="button"
                        variant="outline"
                        size="icon"
                        onClick={() => editor.iniciar(lote)}
                        disabled={!navigator.onLine}
                        aria-label={`Editar lote ${lote.nome}`}
                        className="size-9 rounded-lg border-2 border-zinc-950"
                      >
                        <Pencil className="size-4" aria-hidden="true" />
                      </Button>
                    </div>
                  </article>
                ))}
              </div>
            ) : (
              <div className="rounded-xl border-2 border-dashed border-zinc-300 px-5 py-12 text-center">
                <ClipboardList
                  className="mx-auto size-10 text-zinc-400"
                  aria-hidden="true"
                />
                <p className="mt-3 text-lg font-black uppercase">
                  {busca ? "Nenhum lote encontrado" : "Nenhum lote cadastrado"}
                </p>
                <p className="mt-1 text-sm font-medium text-zinc-600">
                  {busca
                    ? "Tente buscar por outro nome."
                    : "Cadastre o primeiro lote ao lado."}
                </p>
              </div>
            )}
          </section>
        </div>
      </div>
      <EditarLoteDialog editor={editor} />
    </main>
  );
}

function formatarData(valor) {
  if (!valor) return "Data não informada";

  const [ano, mes, dia] = valor.split("-");
  return `${dia}/${mes}/${ano}`;
}
