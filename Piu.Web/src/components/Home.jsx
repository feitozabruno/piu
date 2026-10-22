import { Cloud, CloudOff, Egg, RefreshCw } from "lucide-react";
import { Button } from "@/components/ui/button";
import { ModalRecebimentoOvos } from "@/components/ModalRecebimentoOvos";
import { useState } from "react";
import { useSyncStatus } from "@/hooks/useSyncStatus";

const workAreas = [
  {
    title: "Recepção de ovos",
    status: "8 lotes aguardando",
    statusTone: "green",
    icon: Egg,
    action: "Receber novo lote",
  },
];

export function Home() {
  const [showModalRecebimentoOvos, setShowModalRecebimentoOvos] =
    useState(false);
  const { online, pendentes, sincronizando, falhas, ultimaSincronizacao } =
    useSyncStatus();

  const fila = pendentes + sincronizando + falhas;
  const SyncIcon = online ? Cloud : CloudOff;
  const statusFila =
    falhas > 0 ? "Atenção necessária" : online ? "Conectado" : "Offline";

  return (
    <main className="min-h-[calc(100vh-73px)] bg-[#f4f6f8] text-zinc-950">
      <div className="mx-auto max-w-7xl px-4 py-6 sm:px-8 sm:py-10 lg:px-10">
        <section className="mb-8 flex flex-col gap-4 sm:mb-10 sm:flex-row sm:items-end sm:justify-between">
          <div>
            <p className="mb-2 text-xs font-black uppercase tracking-[0.18em] text-zinc-500">
              Painel de operação
            </p>
            <h1 className="text-3xl font-black uppercase leading-[0.95] tracking-tight sm:text-5xl">
              Selecione a área de trabalho
            </h1>
            <p className="mt-3 max-w-2xl text-base font-medium leading-snug text-zinc-600 sm:text-xl">
              Escolha um setor para iniciar ou continuar as atividades
            </p>
          </div>
          <div className="hidden rounded-full border-2 border-zinc-950 bg-white px-4 py-2 text-xs font-black uppercase tracking-[0.12em] sm:block">
            Operação diária
          </div>
        </section>

        <section aria-label="Áreas de trabalho" className="grid gap-5">
          {workAreas.map((area) => {
            const Icon = area.icon;

            return (
              <article
                key={area.title}
                className="flex flex-col gap-6 rounded-[24px] border-2 border-zinc-950 bg-white p-5 shadow-[6px_6px_0_0_#18181b] sm:flex-row sm:items-center sm:gap-6 sm:p-7"
              >
                <div className="flex size-16 shrink-0 items-center justify-center rounded-2xl border-2 border-zinc-950 bg-[#f6c945] sm:size-20">
                  <Icon
                    aria-hidden="true"
                    className="size-9 sm:size-11"
                    strokeWidth={2.5}
                  />
                </div>
                <div className="min-w-0 flex-1">
                  <h2 className="text-2xl font-black uppercase leading-tight sm:text-3xl">
                    {area.title}
                  </h2>
                  <p
                    className={`mt-3 inline-flex rounded-full px-3 py-1 text-sm font-bold sm:text-base ${area.statusTone === "green" ? "bg-green-100 text-green-800" : area.statusTone === "amber" ? "bg-amber-100 text-amber-700" : "bg-zinc-100 text-zinc-950"}`}
                  >
                    {area.status}
                  </p>
                </div>
                <Button
                  type="button"
                  onClick={() => setShowModalRecebimentoOvos(true)}
                  className={`w-full shrink-0 rounded-xl border-2 border-zinc-950 px-5 py-4 text-base font-black uppercase tracking-[0.06em] text-zinc-950 shadow-[0_4px_0_0_#18181b] transition-all hover:brightness-105 active:translate-y-1 active:shadow-none sm:w-auto sm:px-7 sm:py-5 sm:text-lg ${area.action === "Acessar" ? "bg-green-500" : "bg-[#f6c945]"}`}
                >
                  {area.action}
                </Button>
              </article>
            );
          })}
        </section>

        <section
          className="mt-8 grid gap-5 md:grid-cols-2"
          aria-label="Resumo da operação"
        >
          <article className="rounded-[20px] border-2 border-zinc-950 bg-white p-5 sm:p-6">
            <div className="flex items-start justify-between gap-4">
              <div>
                <p className="text-xs font-black uppercase tracking-[0.16em] text-zinc-500 sm:text-sm">
                  Fila de sincronização
                </p>
                <p className="mt-2 text-5xl font-black tracking-tight sm:text-6xl">
                  {String(fila).padStart(2, "0")}
                </p>
                <p className="mt-1 text-sm font-bold text-zinc-600 sm:text-base">
                  {falhas > 0
                    ? `${falhas} movimentação(ões) com erro`
                    : sincronizando > 0
                      ? "sincronizando agora"
                      : "movimentações pendentes"}
                </p>
              </div>
              <SyncIcon
                aria-hidden="true"
                className={`size-8 ${online ? "text-emerald-600" : "text-zinc-400"}`}
              />
            </div>
            <p className="mt-5 inline-flex rounded-full bg-zinc-100 px-3 py-1 text-xs font-black uppercase tracking-wide text-zinc-600">
              <span
                className={`mr-2 size-2 self-center rounded-full ${falhas > 0 ? "bg-red-500" : online ? "bg-emerald-500" : "bg-zinc-400"}`}
              />
              {statusFila}
            </p>
          </article>
          <article className="rounded-[20px] border-2 border-zinc-950 bg-white p-5 sm:p-6">
            <p className="text-xs font-black uppercase tracking-[0.16em] text-zinc-500 sm:text-sm">
              Última sincronização
            </p>
            <div className="mt-3 flex items-center gap-2">
              {sincronizando > 0 && (
                <RefreshCw className="size-5 animate-spin" aria-hidden="true" />
              )}
              <p className="text-xl font-black leading-tight sm:text-2xl">
                {formatarUltimaSincronizacao(ultimaSincronizacao)}
              </p>
            </div>
            <p className="mt-2 text-sm font-bold leading-snug text-zinc-600 sm:text-base">
              {online
                ? "Dados locais sincronizados automaticamente"
                : "Dados protegidos no dispositivo até a conexão voltar"}
            </p>
          </article>
        </section>

        <ModalRecebimentoOvos
          open={showModalRecebimentoOvos}
          onOpenChange={setShowModalRecebimentoOvos}
        />
      </div>
    </main>
  );
}

function formatarUltimaSincronizacao(valor) {
  if (!valor) return "Ainda não sincronizado";

  return `Sincronizado às ${new Intl.DateTimeFormat("pt-BR", {
    hour: "2-digit",
    minute: "2-digit",
  }).format(new Date(valor))}`;
}
