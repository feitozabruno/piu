import { ArrowLeft, Eye, EyeOff, LockKeyhole, UserRound } from "lucide-react";
import { Link, useNavigate } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { useLogin } from "@/hooks/useLogin";

export function TelaDeLogin() {
  const navigate = useNavigate();

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
    <main className="min-h-screen bg-[#f4f6f8] px-4 py-6 text-zinc-950 sm:px-6 sm:py-10">
      <div className="mx-auto flex min-h-[calc(100vh-3rem)] w-full max-w-5xl overflow-hidden rounded-[28px] border-2 border-zinc-950 bg-white shadow-[8px_8px_0_0_#18181b] sm:min-h-[calc(100vh-5rem)]">
        <aside className="relative hidden w-[42%] flex-col justify-between overflow-hidden bg-[#f6c945] p-10 lg:flex">
          <div className="relative z-10">
            <span className="text-4xl font-black -tracking-widest">Piu</span>
            <p className="mt-16 max-w-xs text-5xl font-black uppercase leading-[0.9] tracking-tight">
              Seu trabalho começa aqui.
            </p>
          </div>
          <p className="relative z-10 max-w-xs text-sm font-bold uppercase leading-snug">
            Acesse o sistema para iniciar seu turno de trabalho.
          </p>
          <div className="absolute -bottom-20 -right-20 size-64 rounded-full border-28 border-zinc-950/10" />
          <div className="absolute -right-10 top-32 size-32 rotate-12 border-18 border-zinc-950/10" />
        </aside>

        <section className="flex w-full flex-col justify-center px-5 py-8 sm:px-12 sm:py-14 lg:w-[58%] lg:px-16">
          <div className="mb-8 lg:mb-10">
            <Link
              to="/"
              className="mb-7 inline-flex items-center gap-2 rounded-lg py-2 text-sm font-black uppercase tracking-[0.06em] text-zinc-600 transition-colors hover:text-zinc-950 focus-visible:outline-4 focus-visible:outline-[#f6c945] focus-visible:outline-offset-2"
            >
              <ArrowLeft aria-hidden="true" className="size-4" />
              Voltar para a Home
            </Link>
            <div className="mb-7 flex items-center gap-3 lg:hidden">
              <span className="rounded-xl border-2 border-zinc-950 bg-[#f6c945] px-3 py-1 text-2xl font-black -tracking-widest">
                Piu
              </span>
              <span className="text-xs font-bold uppercase tracking-[0.16em] text-zinc-500">
                Acesso interno
              </span>
            </div>
            <p className="mb-2 text-sm font-black uppercase tracking-[0.18em] text-zinc-500">
              Bem-vindo de volta
            </p>
            <h2 className="text-4xl font-black uppercase leading-[0.92] tracking-tight sm:text-5xl">
              Acesso ao sistema
            </h2>
            <p className="mt-4 max-w-md text-base font-medium leading-snug text-zinc-600 sm:text-lg">
              Insira suas credenciais para iniciar o turno de trabalho.
            </p>
          </div>

          <form
            onSubmit={handleLoginSubmit}
            className="flex flex-col gap-5 sm:gap-6"
            aria-label="Formulário de acesso"
          >
            <div className="flex flex-col gap-2">
              <Label
                htmlFor="usuario"
                className="text-sm font-black uppercase tracking-[0.08em] text-zinc-700"
              >
                Usuário / ID
              </Label>
              <div className="flex min-h-16 items-center rounded-2xl border-2 border-zinc-950 bg-white px-4 transition-shadow focus-within:ring-4 focus-within:ring-[#f6c945]/70">
                <UserRound
                  aria-hidden="true"
                  className="size-5 shrink-0 text-zinc-600"
                  strokeWidth={2.8}
                />
                <Input
                  id="usuario"
                  name="usuario"
                  type="text"
                  value={formData.usuario}
                  onChange={handleChange}
                  disabled={isLoading}
                  autoComplete="usuario"
                  placeholder="696270"
                  required
                  className="h-auto min-w-0 flex-1 rounded-none border-0 bg-transparent px-3 py-4 text-base font-medium shadow-none outline-none placeholder:text-zinc-400 focus-visible:border-0 focus-visible:ring-0 sm:text-lg"
                />
              </div>
            </div>

            <div className="flex flex-col gap-2">
              <Label
                htmlFor="senha"
                className="text-sm font-black uppercase tracking-[0.08em] text-zinc-700"
              >
                Senha
              </Label>
              <div className="flex min-h-16 items-center rounded-2xl border-2 border-zinc-950 bg-white px-4 transition-shadow focus-within:ring-4 focus-within:ring-[#f6c945]/70">
                <LockKeyhole
                  aria-hidden="true"
                  className="size-5 shrink-0 text-zinc-600"
                  strokeWidth={2.8}
                />
                <Input
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
                  className="h-auto min-w-0 flex-1 rounded-none border-0 bg-transparent px-3 py-4 text-base font-medium shadow-none outline-none placeholder:text-zinc-400 focus-visible:border-0 focus-visible:ring-0 sm:text-lg"
                />
                <Button
                  type="button"
                  onClick={toggleShowPassword}
                  aria-label={showPassword ? "Ocultar senha" : "Mostrar senha"}
                  variant="ghost"
                  size="icon"
                  className="rounded-lg p-2 text-zinc-600 hover:bg-zinc-100 hover:text-zinc-950 focus-visible:ring-[#f6c945]"
                >
                  {showPassword ? (
                    <EyeOff aria-hidden="true" className="size-5" />
                  ) : (
                    <Eye aria-hidden="true" className="size-5" />
                  )}
                </Button>
              </div>
            </div>

            <div className="min-h-5" aria-live="polite">
              {error && (
                <p className="text-center text-sm font-medium text-destructive">
                  {error}
                </p>
              )}
            </div>

            <Button
              type="submit"
              disabled={isLoading}
              className="h-14 rounded-2xl border-2 border-zinc-950 bg-[#f6c945] px-5 text-base font-black uppercase tracking-[0.08em] text-zinc-950 shadow-[0_4px_0_0_#18181b] transition-all hover:bg-[#ffd65c] active:translate-y-1 active:shadow-none sm:h-16 sm:text-lg"
            >
              {isLoading ? "Entrando..." : "Acessar"}
            </Button>
          </form>
        </section>
      </div>
    </main>
  );
}
