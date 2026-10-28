import {
  CheckCircle2,
  Database,
  LoaderCircle,
  Play,
  XCircle,
} from "lucide-react";
import { useEffect, useState } from "react";
import { Button } from "@/shared/components/ui/button";
import { apiFetch } from "@/shared/lib/api";

export function Migrations() {
  const [executando, setExecutando] = useState(false);
  const [status, setStatus] = useState(null);
  const [carregando, setCarregando] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    let mounted = true;

    apiFetch("/migrations")
      .then((resultado) => {
        if (mounted) setStatus(resultado);
      })
      .catch((err) => {
        if (mounted) {
          setError(
            err instanceof Error
              ? err.message
              : "Não foi possível consultar as migrations.",
          );
        }
      })
      .finally(() => {
        if (mounted) setCarregando(false);
      });

    return () => {
      mounted = false;
    };
  }, []);

  async function executarMigrations() {
    setExecutando(true);
    setError(null);

    try {
      const resultado = await apiFetch("/migrations", { method: "POST" });
      setStatus(resultado);
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Não foi possível executar as migrations.",
      );
    } finally {
      setExecutando(false);
    }
  }

  const migrationsExecutadas = status?.executadas ?? [];
  const migrationsPendentes = status?.pendentes ?? [];
  const nenhumaPendente = status !== null && migrationsPendentes.length === 0;

  return (
    <main className="min-h-[calc(100vh-73px)] bg-[#f4f6f8] text-zinc-950">
      <div className="mx-auto max-w-7xl px-4 py-6 sm:px-8 sm:py-10 lg:px-10">
        <header className="mb-8 flex flex-col gap-5 sm:mb-10 sm:flex-row sm:items-end sm:justify-between">
          <div>
            <p className="mb-2 text-xs font-black uppercase tracking-[0.18em] text-zinc-500">
              Administração
            </p>
            <h1 className="text-3xl font-black uppercase leading-[0.95] tracking-tight sm:text-5xl">
              Migrations
            </h1>
            <p className="mt-3 max-w-2xl text-base font-medium leading-snug text-zinc-600 sm:text-xl">
              Atualize a estrutura do banco de dados aplicando as migrations
              pendentes.
            </p>
          </div>
          <div className="flex items-center gap-2 self-start rounded-full border-2 border-zinc-950 bg-white px-4 py-2 text-xs font-black uppercase tracking-[0.12em] sm:self-auto">
            <Database className="size-4" aria-hidden="true" />
            Banco de dados
          </div>
        </header>

        <section className="max-w-3xl rounded-[24px] border-2 border-zinc-950 bg-white p-5 shadow-[6px_6px_0_0_#18181b] sm:p-7">
          <div className="flex flex-col gap-6 sm:flex-row sm:items-start">
            <div className="flex size-16 shrink-0 items-center justify-center rounded-2xl border-2 border-zinc-950 bg-[#f6c945]">
              <Database
                className="size-9"
                strokeWidth={2.5}
                aria-hidden="true"
              />
            </div>
            <div className="min-w-0 flex-1">
              <h2 className="text-2xl font-black uppercase tracking-tight">
                Atualizar banco de dados
              </h2>
              <p className="mt-2 text-base font-medium leading-snug text-zinc-600">
                A execução é segura para repetir. Apenas as migrations que ainda
                não foram aplicadas serão executadas.
              </p>
              <Button
                type="button"
                onClick={() => void executarMigrations()}
                disabled={executando}
                className="mt-6 w-full rounded-xl border-2 border-zinc-950 bg-[#f6c945] px-5 py-4 text-base font-black uppercase tracking-[0.06em] text-zinc-950 shadow-[0_4px_0_0_#18181b] transition-all hover:brightness-105 active:translate-y-1 active:shadow-none disabled:cursor-not-allowed disabled:opacity-60 sm:w-auto"
              >
                {executando ? (
                  <>
                    <LoaderCircle
                      className="size-5 animate-spin"
                      aria-hidden="true"
                    />
                    Executando...
                  </>
                ) : (
                  <>
                    <Play
                      className="size-5"
                      fill="currentColor"
                      aria-hidden="true"
                    />
                    Executar migrations
                  </>
                )}
              </Button>
            </div>
          </div>

          {error && (
            <div
              role="alert"
              className="mt-6 flex items-start gap-3 rounded-xl border-2 border-red-800 bg-red-50 p-4 text-red-900"
            >
              <XCircle className="mt-0.5 size-5 shrink-0" aria-hidden="true" />
              <div>
                <p className="font-black uppercase">
                  Não foi possível atualizar
                </p>
                <p className="mt-1 text-sm font-medium">{error}</p>
              </div>
            </div>
          )}

          {status !== null && (
            <div
              role="status"
              className="mt-6 rounded-xl border-2 border-green-800 bg-green-50 p-4 text-green-900"
            >
              <div className="flex items-start gap-3">
                <CheckCircle2
                  className="mt-0.5 size-5 shrink-0"
                  aria-hidden="true"
                />
                <div>
                  <p className="font-black uppercase">
                    {nenhumaPendente
                      ? "Banco de dados atualizado"
                      : "Migrations executadas com sucesso"}
                  </p>
                  <p className="mt-1 text-sm font-medium">
                    {nenhumaPendente
                      ? "Não havia migrations pendentes."
                      : `${migrationsExecutadas.length} ${
                          migrationsExecutadas.length === 1
                            ? "migration foi executada."
                            : "migrations foram executadas."
                        }`}
                  </p>
                </div>
              </div>
            </div>
          )}

          {carregando && (
            <div className="mt-6 flex items-center gap-2 text-sm font-bold text-zinc-600">
              <LoaderCircle
                className="size-4 animate-spin"
                aria-hidden="true"
              />
              Consultando o estado das migrations...
            </div>
          )}
        </section>

        {status !== null && (
          <section className="mt-6 grid max-w-5xl gap-6 lg:grid-cols-2">
            <MigrationList
              title="Migrations executadas"
              migrations={migrationsExecutadas}
              emptyMessage="Nenhuma migration foi executada ainda."
              className="border-green-800 bg-green-50"
            />
            <MigrationList
              title="Migrations pendentes"
              migrations={migrationsPendentes}
              emptyMessage="Não há migrations pendentes."
              className={
                migrationsPendentes.length > 0
                  ? "border-amber-800 bg-amber-50"
                  : "border-zinc-800 bg-zinc-50"
              }
            />
          </section>
        )}
      </div>
    </main>
  );
}

function MigrationList({ title, migrations, emptyMessage, className }) {
  return (
    <section
      className={`rounded-[24px] border-2 p-5 shadow-[6px_6px_0_0_#18181b] sm:p-7 ${className}`}
    >
      <h2 className="text-xl font-black uppercase tracking-tight">{title}</h2>
      {migrations.length === 0 ? (
        <p className="mt-4 text-sm font-medium text-zinc-600">{emptyMessage}</p>
      ) : (
        <ul className="mt-4 grid gap-2">
          {migrations.map((migration) => (
            <li
              key={migration}
              className="break-all rounded-lg border border-zinc-950/20 bg-white/80 px-3 py-2 font-mono text-xs font-bold"
            >
              {migration}
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}
