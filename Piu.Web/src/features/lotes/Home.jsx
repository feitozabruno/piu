import {
  ArrowUpRight,
  Boxes,
  Cloud,
  CloudOff,
  Egg,
  FileText,
  RefreshCw,
} from "lucide-react";
import { Button } from "@/shared/components/ui/button";
import { ModalRecebimentoOvos } from "@/features/ovos/ModalRecebimentoOvos";
import { useState } from "react";
import { useSyncStatus } from "@/infrastructure/offline/useSyncStatus";
import { NavLink } from "react-router-dom";

const workAreas = [
  {
    title: "Recepção de ovos",
    status: "8 lotes aguardando",
    statusTone: "green",
    icon: Egg,
    action: "Receber novo lote",
  },
];

const featureShortcuts = [
  {
    to: "/lotes",
    title: "Lotes",
    description: "Consulte e acompanhe os lotes cadastrados.",
    icon: Boxes,
    iconClassName: "bg-sky-100 text-sky-800",
  },
  {
    to: "/ovos",
    title: "Ovos recebidos",
    description: "Registre recebimentos e veja o histórico.",
    icon: Egg,
    iconClassName: "bg-[#f6c945] text-zinc-950",
  },
  {
    to: "/relatorios",
    title: "Relatórios",
    description: "Analise os dados da operação.",
    icon: FileText,
    iconClassName: "bg-violet-100 text-violet-800",
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
        <section className="mb-8 flex flex-col gap-5 sm:mb-10 sm:flex-row sm:items-end sm:justify-between">
          <div>
            <p className="mb-2 text-xs font-black uppercase tracking-[0.18em] text-zinc-500">
              Visão geral
            </p>
            <h1 className="text-3xl font-black uppercase leading-[0.95] tracking-tight sm:text-5xl">
              Bom trabalho hoje
            </h1>
            <p className="mt-3 max-w-2xl text-base font-medium leading-snug text-zinc-600 sm:text-xl">
              Acompanhe a operação e acesse rapidamente as principais
              funcionalidades.
            </p>
          </div>
          <div className="w-fit rounded-full border-2 border-zinc-950 bg-white px-4 py-2 text-xs font-black uppercase tracking-[0.12em]">
            Operação diária
          </div>
        </section>

        <section aria-labelledby="atalhos-title" className="mb-8">
          <div className="mb-4 flex items-end justify-between gap-4">
            <div>
              <p className="text-xs font-black uppercase tracking-[0.16em] text-zinc-500">
                Navegação rápida
              </p>
              <h2
                id="atalhos-title"
                className="mt-1 text-2xl font-black uppercase tracking-tight sm:text-3xl"
              >
                Acesse uma funcionalidade
              </h2>
            </div>
          </div>
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {featureShortcuts.map(
              ({ to, title, description, icon: Icon, iconClassName }) => (
                <NavLink
                  key={to}
                  to={to}
                  className="group flex min-h-36 flex-col justify-between rounded-[20px] border-2 border-zinc-950 bg-white p-5 shadow-[4px_4px_0_0_#18181b] transition-transform hover:-translate-y-1 focus-visible:outline-4 focus-visible:outline-[#f6c945] focus-visible:outline-offset-2 active:translate-y-0 active:shadow-none"
                >
                  <div className="flex items-start justify-between gap-4">
                    <div
                      className={`flex size-11 items-center justify-center rounded-xl border-2 border-zinc-950 ${iconClassName}`}
                    >
                      <Icon
                        aria-hidden="true"
                        className="size-6"
                        strokeWidth={2.5}
                      />
                    </div>
                    <ArrowUpRight
                      aria-hidden="true"
                      className="size-5 text-zinc-400 transition-transform group-hover:-translate-y-0.5 group-hover:translate-x-0.5 group-hover:text-zinc-950"
                      strokeWidth={2.5}
                    />
                  </div>
                  <div className="mt-5">
                    <h3 className="text-lg font-black uppercase tracking-tight">
                      {title}
                    </h3>
                    <p className="mt-1 text-sm font-medium leading-snug text-zinc-600">
                      {description}
                    </p>
                  </div>
                </NavLink>
              ),
            )}
          </div>
        </section>

        <section aria-labelledby="area-trabalho-title">
          <div className="mb-4">
            <p className="text-xs font-black uppercase tracking-[0.16em] text-zinc-500">
              Ação principal
            </p>
            <h2
              id="area-trabalho-title"
              className="mt-1 text-2xl font-black uppercase tracking-tight sm:text-3xl"
            >
              Comece uma atividade
            </h2>
          </div>
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
