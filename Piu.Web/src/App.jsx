import { CloudOff, Egg, LogOut, Wifi, WifiOff } from "lucide-react";
import { Button } from "@/components/ui/button";
import { ModalRecebimentoOvos } from "@/components/ModalRecebimentoOvos";
import { useEffect, useState } from "react";

const workAreas = [
  {
    title: "Sala de ovos",
    status: "8 lotes aguardando",
    statusTone: "green",
    icon: Egg,
    action: "Receber novo lote",
  },
];

function App() {
  const [isOnline, setIsOnline] = useState(true);
  const [showModalRecebimentoOvos, setShowModalRecebimentoOvos] =
    useState(false);

  useEffect(() => {
    const updateConnection = () => setIsOnline(window.navigator.onLine);
    updateConnection();
    window.addEventListener("online", updateConnection);
    window.addEventListener("offline", updateConnection);
    return () => {
      window.removeEventListener("online", updateConnection);
      window.removeEventListener("offline", updateConnection);
    };
  }, []);

  return (
    <main className="min-h-screen bg-[#f4f6f8] text-zinc-950">
      <header className="border-b-4 border-zinc-950 bg-white px-5 py-5 sm:px-10">
        <div className="mx-auto flex max-w-7xl items-center justify-between gap-4">
          <h1 className="text-5xl font-black tracking-[-0.08em] sm:text-6xl">
            Piu
          </h1>
          <div className="flex items-center gap-3 sm:gap-8">
            <p className="hidden text-lg uppercase sm:block sm:text-2xl">
              Operador: <strong>Carlos</strong>
            </p>
            <div
              aria-live="polite"
              className={`flex items-center gap-2 rounded-xl border-4 px-3 py-2 text-base font-black uppercase sm:px-5 sm:text-xl ${isOnline ? "border-green-700 bg-green-50 text-green-800" : "border-amber-700 bg-amber-50 text-amber-900"}`}
            >
              {isOnline ? (
                <Wifi aria-hidden="true" />
              ) : (
                <WifiOff aria-hidden="true" />
              )}
              <span>{isOnline ? "Online" : "Offline"}</span>
            </div>
            <Button
              variant="ghost"
              size="icon"
              aria-label="Sair"
              className="hidden sm:inline-flex"
            >
              <LogOut aria-hidden="true" />
            </Button>
          </div>
        </div>
      </header>

      <div className="mx-auto max-w-7xl px-5 py-8 sm:px-10 sm:py-11">
        <section className="mb-7">
          <p className="mb-1 text-3xl font-black uppercase leading-tight sm:text-5xl">
            Selecione a área de trabalho
          </p>
          <p className="text-lg font-medium sm:text-2xl">
            Escolha um setor para iniciar ou continuar as atividades
          </p>
        </section>

        <section
          aria-label="Áreas de trabalho"
          className="grid gap-5 lg:grid-cols-2"
        >
          {workAreas.map((area) => {
            const Icon = area.icon;

            return (
              <article
                key={area.title}
                className="flex min-h-48 items-center gap-5 rounded-2xl border-4 border-zinc-950 bg-white px-5 py-6 shadow-[0_7px_0_0_#18181b] sm:px-8"
              >
                <Icon
                  aria-hidden="true"
                  className="size-16 shrink-0 sm:size-20"
                  strokeWidth={2.5}
                />
                <div className="min-w-0 flex-1 self-stretch">
                  <div className="flex h-full flex-col justify-between gap-5 sm:flex-row sm:items-center sm:gap-7">
                    <div>
                      <h2 className="text-2xl font-black uppercase leading-tight sm:text-3xl">
                        {area.title}
                      </h2>
                      <p
                        className={`mt-2 inline-block px-1 text-xl font-mono sm:text-2xl ${area.statusTone === "green" ? "bg-green-100 text-green-800" : area.statusTone === "amber" ? "bg-amber-100 text-amber-700" : "bg-zinc-100 text-zinc-950"}`}
                      >
                        {area.status}
                      </p>
                    </div>
                    <Button
                      type="button"
                      onClick={() => setShowModalRecebimentoOvos(true)}
                      className={`shrink-0 rounded-xl border-4 border-zinc-950 px-5 py-6 text-xl font-black uppercase text-zinc-950 shadow-[0_5px_0_0_#18181b] hover:brightness-105 active:translate-y-1 active:shadow-[0_2px_0_0_#18181b] ${area.action === "Acessar" ? "bg-green-500" : "bg-[#f6c945]"}`}
                    >
                      {area.action}
                    </Button>
                  </div>
                </div>
              </article>
            );
          })}
        </section>

        <section
          className="mt-7 grid gap-5 md:grid-cols-2"
          aria-label="Resumo da operação"
        >
          <article className="rounded-2xl border-2 border-zinc-950 bg-white p-6">
            <div className="flex items-start justify-between gap-4">
              <div>
                <p className="text-sm font-black uppercase tracking-[0.16em] text-zinc-500">
                  Fila de sincronização
                </p>
                <p className="mt-3 text-5xl font-black">00</p>
                <p className="mt-1 font-bold text-zinc-600">
                  movimentações pendentes
                </p>
              </div>
              <CloudOff aria-hidden="true" className="size-9 text-zinc-400" />
            </div>
          </article>
          <article className="rounded-2xl border-2 border-zinc-950 bg-white p-6">
            <p className="text-sm font-black uppercase tracking-[0.16em] text-zinc-500">
              Última sincronização
            </p>
            <p className="mt-3 text-2xl font-black">Sincronizado agora</p>
            <p className="mt-1 font-bold text-zinc-600">
              Seus dados estão protegidos no dispositivo
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

export default App;
