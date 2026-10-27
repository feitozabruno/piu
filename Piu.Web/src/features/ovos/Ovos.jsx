import { useMemo, useState } from "react";
import {
  CalendarDays,
  Egg,
  LoaderCircle,
  Plus,
  RefreshCw,
  Search,
} from "lucide-react";
import { Button } from "@/shared/components/ui/button";
import { Input } from "@/shared/components/ui/input";
import { ModalRecebimentoOvos } from "@/features/ovos/ModalRecebimentoOvos";
import { useOvosRecebidos } from "@/features/ovos/useOvosRecebidos";

export function Ovos() {
  const hoje = formatarData(new Date());
  const [dataInicial, setDataInicial] = useState(hoje);
  const [dataFinal, setDataFinal] = useState(hoje);
  const [busca, setBusca] = useState("");
  const [modalAberto, setModalAberto] = useState(false);
  const { ovos, error, carregando, carregarOvos } = useOvosRecebidos(
    dataInicial,
    dataFinal,
  );

  const ovosFiltrados = useMemo(() => {
    const termo = busca.trim().toLocaleLowerCase();
    if (!termo) return ovos;

    return ovos.filter((ovo) =>
      [ovo.lote, ovo.motorista, ovo.placaCaminhao].some((valor) =>
        valor?.toLocaleLowerCase().includes(termo),
      ),
    );
  }, [busca, ovos]);

  const totalRecebido = ovosFiltrados.reduce(
    (total, ovo) => total + (ovo.totalRecebido ?? 0),
    0,
  );

  async function atualizarLista() {
    await carregarOvos(dataInicial, dataFinal);
  }

  return (
    <main className="min-h-[calc(100vh-73px)] bg-[#f4f6f8] text-zinc-950">
      <div className="mx-auto max-w-7xl px-4 py-6 sm:px-8 sm:py-10 lg:px-10">
        <header className="mb-8 flex flex-col gap-5 sm:mb-10 sm:flex-row sm:items-end sm:justify-between">
          <div>
            <p className="mb-2 text-xs font-black uppercase tracking-[0.18em] text-zinc-500">
              Controle de recebimento
            </p>
            <h1 className="text-3xl font-black uppercase leading-[0.95] tracking-tight sm:text-5xl">
              Ovos recebidos
            </h1>
            <p className="mt-3 max-w-2xl text-base font-medium leading-snug text-zinc-600 sm:text-xl">
              Consulte os recebimentos e acompanhe a operação diária.
            </p>
          </div>
          <Button
            type="button"
            onClick={() => setModalAberto(true)}
            className="h-12 rounded-xl border-2 border-zinc-950 bg-[#f6c945] font-black uppercase text-zinc-950 shadow-[0_4px_0_0_#18181b] hover:bg-[#ffd95f] active:translate-y-1 active:shadow-none"
          >
            <Plus className="size-5" aria-hidden="true" />
            Receber ovos
          </Button>
        </header>

        <section className="mb-6 grid gap-4 sm:grid-cols-3">
          <Resumo label="Recebimentos" valor={ovosFiltrados.length} />
          <Resumo
            label="Ovos recebidos"
            valor={totalRecebido.toLocaleString("pt-BR")}
          />
          <Resumo
            label="Lotes movimentados"
            valor={new Set(ovosFiltrados.map((ovo) => ovo.lote)).size}
          />
        </section>

        <section className="rounded-[24px] border-2 border-zinc-950 bg-white p-5 shadow-[6px_6px_0_0_#18181b] sm:p-7">
          <div className="mb-6 flex flex-col gap-4 lg:flex-row lg:items-end lg:justify-between">
            <div>
              <h2 className="text-xl font-black uppercase">
                Histórico de recebimentos
              </h2>
              <p className="mt-1 text-sm font-medium text-zinc-600">
                Filtre por período, lote, motorista ou placa.
              </p>
            </div>
            <Button
              type="button"
              variant="outline"
              onClick={atualizarLista}
              disabled={carregando}
              className="h-10 rounded-xl border-2 border-zinc-950 px-3 font-black uppercase"
            >
              <RefreshCw
                className={`size-4 ${carregando ? "animate-spin" : ""}`}
              />
              Atualizar
            </Button>
          </div>

          <div className="mb-6 grid gap-3 md:grid-cols-[1fr_1fr_1.5fr]">
            <CampoData
              label="De"
              valor={dataInicial}
              onChange={setDataInicial}
            />
            <CampoData label="Até" valor={dataFinal} onChange={setDataFinal} />
            <label className="relative grid gap-2 text-sm font-black uppercase tracking-wide">
              Buscar
              <Search
                className="absolute bottom-3 left-3 size-5 text-zinc-500"
                aria-hidden="true"
              />
              <Input
                value={busca}
                onChange={(event) => setBusca(event.target.value)}
                placeholder="Lote, motorista ou placa"
                className="pl-10"
              />
            </label>
          </div>

          {error && (
            <p
              className="mb-4 rounded-xl border-2 border-red-700 bg-red-50 px-3 py-2 text-sm font-bold text-red-800"
              role="alert"
            >
              {error}
            </p>
          )}
          {carregando ? (
            <div className="flex items-center justify-center gap-2 py-14 font-bold text-zinc-600">
              <LoaderCircle className="size-5 animate-spin" /> Carregando
              recebimentos...
            </div>
          ) : ovosFiltrados.length === 0 ? (
            <div className="rounded-2xl border-2 border-dashed border-zinc-300 px-5 py-14 text-center">
              <Egg
                className="mx-auto size-10 text-zinc-400"
                aria-hidden="true"
              />
              <p className="mt-3 font-black uppercase">
                Nenhum recebimento encontrado
              </p>
              <p className="mt-1 text-sm font-medium text-zinc-500">
                Ajuste os filtros ou registre um novo recebimento.
              </p>
            </div>
          ) : (
            <div className="overflow-x-auto">
              <table className="w-full min-w-[720px] border-collapse text-left">
                <thead>
                  <tr className="border-b-2 border-zinc-950 text-xs font-black uppercase tracking-wide text-zinc-500">
                    <th className="px-3 py-3">Data / hora</th>
                    <th className="px-3 py-3">Lote</th>
                    <th className="px-3 py-3">Quantidade</th>
                    <th className="px-3 py-3">Motorista</th>
                    <th className="px-3 py-3">Placa</th>
                    <th className="px-3 py-3">Temperaturas</th>
                  </tr>
                </thead>
                <tbody>
                  {ovosFiltrados.map((ovo) => (
                    <tr
                      key={ovo.id}
                      className="border-b border-zinc-200 text-sm font-bold last:border-0"
                    >
                      <td className="px-3 py-4">
                        {formatarDataHora(ovo.horaRecebimento)}
                      </td>
                      <td className="px-3 py-4">{ovo.lote}</td>
                      <td className="px-3 py-4">
                        <span className="rounded-full bg-[#f6c945] px-2 py-1">
                          {(ovo.totalRecebido ?? 0).toLocaleString("pt-BR")}
                        </span>
                      </td>
                      <td className="px-3 py-4">{ovo.motorista}</td>
                      <td className="px-3 py-4">{ovo.placaCaminhao}</td>
                      <td className="px-3 py-4">
                        {ovo.temperaturaRecebimento}° / {ovo.temperaturaBau}°
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </section>
      </div>
      <ModalRecebimentoOvos open={modalAberto} onOpenChange={setModalAberto} />
    </main>
  );
}

function Resumo({ label, valor }) {
  return (
    <article className="rounded-[20px] border-2 border-zinc-950 bg-white p-5">
      <p className="text-xs font-black uppercase tracking-[0.14em] text-zinc-500">
        {label}
      </p>
      <p className="mt-2 text-4xl font-black">{valor}</p>
    </article>
  );
}

function CampoData({ label, valor, onChange }) {
  return (
    <label className="grid gap-2 text-sm font-black uppercase tracking-wide">
      {label}
      <div className="flex items-center rounded-xl border-2 border-zinc-950 px-3">
        <CalendarDays className="size-4 text-zinc-500" />
        <Input
          type="date"
          value={valor}
          onChange={(event) => onChange(event.target.value)}
          className="border-0 shadow-none focus-visible:ring-0"
        />
      </div>
    </label>
  );
}

function formatarData(valor) {
  return [
    valor.getFullYear(),
    String(valor.getMonth() + 1).padStart(2, "0"),
    String(valor.getDate()).padStart(2, "0"),
  ].join("-");
}

function formatarDataHora(valor) {
  if (!valor) return "-";
  return new Intl.DateTimeFormat("pt-BR", {
    dateStyle: "short",
    timeStyle: "short",
  }).format(new Date(valor));
}
