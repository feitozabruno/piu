import { useEffect, useState } from "react";
import {
  Eye,
  EyeOff,
  LockKeyhole,
  UserRound,
  Wifi,
  WifiOff,
} from "lucide-react";
import { useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { useLogin } from "@/hooks/useLogin";

export function TelaDeLogin() {
  const navigate = useNavigate();
  const [isOnline, setIsOnline] = useState(true);

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

  const {
    formData,
    isLoading,
    error,
    showPassword,
    handleChange,
    handleSubmit,
    toggleShowPassword,
  } = useLogin();

  async function handleLoginSubmit(event) {
    const isAuthenticated = await handleSubmit(event);

    if (isAuthenticated) {
      navigate("/", { replace: true });
    }
  }

  return (
    <main className="min-h-screen bg-[#f4f6f8] text-zinc-950">
      <div className="mx-auto flex min-h-screen w-full max-w-140 flex-col bg-white shadow-[0_0_36px_rgba(0,0,0,0.08)] sm:min-h-215 sm:my-8 sm:rounded-[28px] sm:border-4 sm:border-zinc-950 sm:overflow-hidden">
        <header className="flex items-center justify-between gap-3 border-b-4 border-zinc-950 bg-white px-4 py-5 sm:px-9 sm:py-7">
          <h1 className="text-5xl font-black leading-none tracking-[-0.09em] sm:text-6xl">
            Piu
          </h1>
          <span className="rounded-lg border-4 border-zinc-950 px-2 py-2 text-[0.65rem] font-black uppercase leading-tight sm:px-3 sm:text-lg">
            Sistema operacional
          </span>
        </header>

        <section className="flex flex-1 flex-col px-4 py-8 sm:px-9 sm:py-16">
          <div>
            <h2 className="text-3xl font-black uppercase leading-[0.95] tracking-tight sm:text-5xl">
              Acesso ao sistema
            </h2>
            <p className="mt-3 max-w-md text-lg font-medium leading-tight sm:text-2xl">
              Insira suas credenciais para iniciar o turno de trabalho.
            </p>
          </div>

          <form
            onSubmit={handleLoginSubmit}
            className="mt-10 flex flex-col gap-6 sm:mt-14 sm:gap-7"
            aria-label="Formulário de acesso"
          >
            <div className="flex flex-col gap-2">
              <label
                htmlFor="usuario"
                className="text-xl font-black uppercase leading-none sm:text-2xl"
              >
                Usuário / ID
              </label>
              <div className="flex items-center rounded-2xl border-4 border-zinc-950 bg-white px-4 focus-within:ring-4 focus-within:ring-[#f6c945]/70">
                <UserRound
                  aria-hidden="true"
                  className="size-8 shrink-0"
                  strokeWidth={2.8}
                />
                <input
                  id="usuario"
                  name="usuario"
                  type="text"
                  value={formData.usuario}
                  onChange={handleChange}
                  disabled={isLoading}
                  autoComplete="usuario"
                  placeholder="696270"
                  required
                  className="min-w-0 flex-1 bg-transparent px-3 py-4 text-lg outline-none placeholder:text-zinc-500 sm:px-4 sm:text-xl"
                />
              </div>
            </div>

            <div className="flex flex-col gap-2">
              <label
                htmlFor="senha"
                className="text-2xl font-black uppercase leading-none"
              >
                Senha
              </label>
              <div className="flex items-center rounded-2xl border-4 border-zinc-950 bg-white px-4 focus-within:ring-4 focus-within:ring-[#f6c945]/70">
                <LockKeyhole
                  aria-hidden="true"
                  className="size-8 shrink-0"
                  strokeWidth={2.8}
                />
                <input
                  id="senha"
                  name="senha"
                  type={showPassword ? "text" : "password"}
                  minLength={6}
                  value={formData.senha}
                  onChange={handleChange}
                  disabled={isLoading}
                  autoComplete="senha"
                  placeholder="••••••"
                  required
                  className="min-w-0 flex-1 bg-transparent px-3 py-4 text-lg outline-none placeholder:text-zinc-500 sm:px-4 sm:text-xl"
                />
                <button
                  type="button"
                  onClick={toggleShowPassword}
                  aria-label={showPassword ? "Ocultar senha" : "Mostrar senha"}
                  className="rounded-lg p-1 hover:bg-zinc-100 focus-visible:outline-4 focus-visible:outline-[#f6c945]"
                >
                  {showPassword ? (
                    <EyeOff aria-hidden="true" className="size-8" />
                  ) : (
                    <Eye aria-hidden="true" className="size-8" />
                  )}
                </button>
              </div>
            </div>

            <div className="flex h-0 items-center justify-center">
              {error && (
                <p className="text-destructive text-center text-sm">{error}</p>
              )}
            </div>

            <Button
              type="submit"
              disabled={isLoading}
              className="mt-1 h-auto rounded-2xl border-4 border-zinc-950 bg-[#f6c945] px-5 py-4 text-xl font-black uppercase text-zinc-950 shadow-[0_6px_0_0_#18181b] hover:bg-[#ffd65c] active:translate-y-1 active:shadow-[0_2px_0_0_#18181b] sm:py-5 sm:text-2xl"
            >
              {isLoading ? "Entrando..." : "Acessar"}
            </Button>
          </form>

          <div className="mt-auto flex flex-col items-center gap-2 pt-12 text-center sm:pt-16">
            <p className="flex items-center gap-2 text-base font-semibold sm:text-lg">
              {isOnline ? (
                <Wifi aria-hidden="true" className="size-6 text-green-600" />
              ) : (
                <WifiOff aria-hidden="true" className="size-6 text-amber-600" />
              )}
              <span>
                {isOnline
                  ? "Sincronização Offline Ativa"
                  : "Sem conexão — modo offline"}
              </span>
            </p>
            <span className="rounded-md border-2 border-zinc-300 px-4 py-1 text-sm font-bold uppercase text-zinc-800">
              Pronto para uso offline
            </span>
          </div>
        </section>
      </div>
    </main>
  );
}
