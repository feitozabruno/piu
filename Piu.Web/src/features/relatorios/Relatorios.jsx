import { CalendarDays, Download, FileText } from "lucide-react";
import { useState } from "react";
import { Button } from "@/shared/components/ui/button";
import { Input } from "@/shared/components/ui/input";
import { Label } from "@/shared/components/ui/label";
import { apiFetchBlob } from "@/shared/lib/api";

const DATA_MAXIMA_INTERVALO = 31;

export function Relatorios() {
  const hoje = formatarData(new Date());
  const [dataInicial, setDataInicial] = useState(hoje);
  const [dataFinal, setDataFinal] = useState(hoje);
  const [isLoading, setIsLoading] = useState(false);
  const [mensagem, setMensagem] = useState(null);

  async function handleSubmit(event) {
    event.preventDefault();
    setMensagem(null);

    const inicio = parseData(dataInicial);
    const fim = parseData(dataFinal);

    if (!inicio || !fim || inicio > fim) {
      setMensagem({
        tipo: "erro",
        texto: "Informe um intervalo de datas válido.",
      });
      return;
    }

    const quantidadeDias = Math.floor((fim - inicio) / 86400000) + 1;
    if (quantidadeDias > DATA_MAXIMA_INTERVALO) {
      setMensagem({
        tipo: "erro",
        texto: "O intervalo não pode ultrapassar 31 dias.",
      });
      return;
    }

    setIsLoading(true);

    try {
      const query = new URLSearchParams({
        dataInicial,
        dataFinal,
      });
      const arquivo = await apiFetchBlob(`/relatorios/ovos?${query}`);

      if (!arquivo) {
        setMensagem({
          tipo: "aviso",
          texto: "Nenhum recebimento encontrado nesse período.",
        });
        return;
      }

      const url = URL.createObjectURL(arquivo);
      const link = document.createElement("a");
      link.href = url;
      link.download = `controle-ovos-${dataInicial}-a-${dataFinal}.pdf`;
      document.body.appendChild(link);
      link.click();
      link.remove();
      URL.revokeObjectURL(url);
      setMensagem({
        tipo: "sucesso",
        texto: "Relatório gerado e baixado com sucesso.",
      });
    } catch (error) {
      setMensagem({
        tipo: "erro",
        texto:
          error instanceof Error
            ? error.message
            : "Não foi possível gerar o relatório.",
      });
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <main className="min-h-[calc(100vh-73px)] bg-[#f4f6f8] text-zinc-950">
      <div className="mx-auto max-w-7xl px-4 py-6 sm:px-8 sm:py-10 lg:px-10">
        <section className="mb-8 flex flex-col gap-4 sm:mb-10 sm:flex-row sm:items-end sm:justify-between">
          <div>
            <p className="mb-2 text-xs font-black uppercase tracking-[0.18em] text-zinc-500">
              Documentos da operação
            </p>
            <h1 className="text-3xl font-black uppercase leading-[0.95] tracking-tight sm:text-5xl">
              Relatórios
            </h1>
            <p className="mt-3 max-w-2xl text-base font-medium leading-snug text-zinc-600 sm:text-xl">
              Gere o controle de ovos recebidos por período.
            </p>
          </div>
          <div className="hidden rounded-full border-2 border-zinc-950 bg-white px-4 py-2 text-xs font-black uppercase tracking-[0.12em] sm:block">
            Exportação em PDF
          </div>
        </section>

        <section className="grid gap-5 lg:grid-cols-[minmax(0,1.35fr)_minmax(280px,0.65fr)]">
          <form
            onSubmit={handleSubmit}
            className="rounded-[24px] border-2 border-zinc-950 bg-white p-5 shadow-[6px_6px_0_0_#18181b] sm:p-8"
            aria-label="Gerar relatório de ovos"
          >
            <div className="mb-8 flex items-start gap-4">
              <div className="flex size-14 shrink-0 items-center justify-center rounded-2xl border-2 border-zinc-950 bg-[#f6c945]">
                <FileText
                  aria-hidden="true"
                  className="size-7"
                  strokeWidth={2.5}
                />
              </div>
              <div>
                <h2 className="text-2xl font-black uppercase leading-tight sm:text-3xl">
                  Controle de ovos
                </h2>
                <p className="mt-2 text-sm font-medium leading-snug text-zinc-600 sm:text-base">
                  Selecione as datas para baixar o relatório preenchido.
                </p>
              </div>
            </div>

            <div className="grid gap-5 sm:grid-cols-2">
              <div className="flex flex-col gap-2">
                <Label
                  htmlFor="data-inicial"
                  className="text-sm font-black uppercase tracking-[0.08em] text-zinc-700"
                >
                  Data inicial
                </Label>
                <div className="flex min-h-14 items-center rounded-2xl border-2 border-zinc-950 bg-white px-4 transition-shadow focus-within:ring-4 focus-within:ring-[#f6c945]/70">
                  <CalendarDays
                    aria-hidden="true"
                    className="size-5 shrink-0 text-zinc-600"
                  />
                  <Input
                    id="data-inicial"
                    type="date"
                    value={dataInicial}
                    onChange={(event) => setDataInicial(event.target.value)}
                    disabled={isLoading}
                    required
                    className="h-auto border-0 bg-transparent px-3 py-3 font-bold shadow-none outline-none focus-visible:border-0 focus-visible:ring-0"
                  />
                </div>
              </div>
              <div className="flex flex-col gap-2">
                <Label
                  htmlFor="data-final"
                  className="text-sm font-black uppercase tracking-[0.08em] text-zinc-700"
                >
                  Data final
                </Label>
                <div className="flex min-h-14 items-center rounded-2xl border-2 border-zinc-950 bg-white px-4 transition-shadow focus-within:ring-4 focus-within:ring-[#f6c945]/70">
                  <CalendarDays
                    aria-hidden="true"
                    className="size-5 shrink-0 text-zinc-600"
                  />
                  <Input
                    id="data-final"
                    type="date"
                    value={dataFinal}
                    onChange={(event) => setDataFinal(event.target.value)}
                    disabled={isLoading}
                    required
                    className="h-auto border-0 bg-transparent px-3 py-3 font-bold shadow-none outline-none focus-visible:border-0 focus-visible:ring-0"
                  />
                </div>
              </div>
            </div>

            <p className="mt-4 text-sm font-bold text-zinc-500">
              O período pode ter no máximo 31 dias.
            </p>

            <div className="mt-7 min-h-6" aria-live="polite">
              {mensagem && (
                <p
                  className={`text-sm font-bold ${
                    mensagem.tipo === "erro"
                      ? "text-red-700"
                      : mensagem.tipo === "sucesso"
                        ? "text-emerald-700"
                        : "text-amber-700"
                  }`}
                >
                  {mensagem.texto}
                </p>
              )}
            </div>

            <Button
              type="submit"
              disabled={isLoading}
              className="mt-4 h-14 w-full rounded-2xl border-2 border-zinc-950 bg-[#f6c945] px-5 text-base font-black uppercase tracking-[0.08em] text-zinc-950 shadow-[0_4px_0_0_#18181b] transition-all hover:bg-[#ffd65c] active:translate-y-1 active:shadow-none sm:text-lg"
            >
              <Download aria-hidden="true" className="size-5" />
              {isLoading ? "Gerando..." : "Gerar relatório PDF"}
            </Button>
          </form>

          <aside className="rounded-[20px] border-2 border-zinc-950 bg-zinc-950 p-5 text-white sm:p-6">
            <p className="text-xs font-black uppercase tracking-[0.16em] text-[#f6c945] sm:text-sm">
              Como funciona
            </p>
            <ol className="mt-5 space-y-5">
              <li className="flex gap-3">
                <span className="flex size-7 shrink-0 items-center justify-center rounded-full bg-[#f6c945] text-sm font-black text-zinc-950">
                  1
                </span>
                <p className="text-sm font-bold leading-snug text-zinc-200">
                  Escolha o primeiro e o último dia do período.
                </p>
              </li>
              <li className="flex gap-3">
                <span className="flex size-7 shrink-0 items-center justify-center rounded-full bg-[#f6c945] text-sm font-black text-zinc-950">
                  2
                </span>
                <p className="text-sm font-bold leading-snug text-zinc-200">
                  Clique em gerar para preparar o documento.
                </p>
              </li>
              <li className="flex gap-3">
                <span className="flex size-7 shrink-0 items-center justify-center rounded-full bg-[#f6c945] text-sm font-black text-zinc-950">
                  3
                </span>
                <p className="text-sm font-bold leading-snug text-zinc-200">
                  O PDF será baixado automaticamente.
                </p>
              </li>
            </ol>
          </aside>
        </section>
      </div>
    </main>
  );
}

function formatarData(data) {
  const ano = data.getFullYear();
  const mes = String(data.getMonth() + 1).padStart(2, "0");
  const dia = String(data.getDate()).padStart(2, "0");
  return `${ano}-${mes}-${dia}`;
}

function parseData(valor) {
  const partes = valor.match(/^(\d{4})-(\d{2})-(\d{2})$/);
  if (!partes) return null;

  const data = new Date(`${valor}T00:00:00`);
  if (
    Number.isNaN(data.getTime()) ||
    data.getFullYear() !== Number(partes[1]) ||
    data.getMonth() + 1 !== Number(partes[2]) ||
    data.getDate() !== Number(partes[3])
  ) {
    return null;
  }

  return data;
}
