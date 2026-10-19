import { useEffect, useState } from "react";
import { Egg, LogOut, Thermometer, Truck, Wifi, WifiOff } from "lucide-react";

const workAreas = [
  {
    title: "Recepção de ovos",
    status: "4 lotes aguardando",
    statusTone: "green",
    icon: Egg,
    action: "Entrar",
  },
  {
    title: "Incubadoras",
    status: "Atenção: Temp. crítica",
    statusTone: "amber",
    icon: Egg,
    action: "Entrar",
  },
  {
    title: "Nascedouros",
    status: "2 movimentações hoje",
    statusTone: "neutral",
    icon: Thermometer,
    action: "Entrar",
  },
  {
    title: "Expedição / Vacinação",
    status: "2 movimentações hoje",
    statusTone: "neutral",
    icon: Truck,
    action: "Acessar",
  },
];

export function OperatorDashboard() {
  const [isOnline, setIsOnline] = useState(true);
  // const [showNewReceipt, setShowNewReceipt] = useState(false);

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

  // if (showNewReceipt)
  //   return <NewReceiptForm onBack={() => setShowNewReceipt(false)} />;

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
            <button
              variant="ghost"
              size="icon"
              aria-label="Sair"
              className="hidden sm:inline-flex"
            >
              <LogOut aria-hidden="true" />
            </button>
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
            const isReceipt = area.title === "Recepção de ovos";
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
                      onClick={
                        isReceipt ? () => setShowNewReceipt(true) : undefined
                      }
                      className={`shrink-0 rounded-xl border-4 border-zinc-950 px-5 py-4 text-xl font-black uppercase text-zinc-950 shadow-[0_5px_0_0_#18181b] hover:brightness-105 active:translate-y-1 active:shadow-[0_2px_0_0_#18181b] ${area.action === "Acessar" ? "bg-green-500" : "bg-[#f6c945]"}`}
                    >
                      {area.action}
                    </Button>
                  </div>
                </div>
              </article>
            );
          })}
        </section>
      </div>
    </main>
  );
}
