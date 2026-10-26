import { useState } from "react";
import { UserPlus } from "lucide-react";
import { apiFetch } from "@/shared/lib/api";
import { Button } from "@/shared/components/ui/button";
import { Input } from "@/shared/components/ui/input";
import { Label } from "@/shared/components/ui/label";

export function PainelGerencia() {
  const [form, setForm] = useState({
    usuario: "",
    senha: "",
    nome: "",
    cargo: "",
  });
  const [message, setMessage] = useState(null);
  const [loading, setLoading] = useState(false);
  const change = (event) =>
    setForm((current) => ({
      ...current,
      [event.target.name]: event.target.value,
    }));
  async function submit(event) {
    event.preventDefault();
    setLoading(true);
    setMessage(null);
    try {
      await apiFetch("/gerente/funcionarios", {
        method: "POST",
        body: JSON.stringify(form),
      });
      setForm({ usuario: "", senha: "", nome: "", cargo: "" });
      setMessage("Funcionário cadastrado com acesso ao seu incubatório.");
    } catch (error) {
      setMessage(error.message);
    } finally {
      setLoading(false);
    }
  }
  return (
    <main className="min-h-[calc(100vh-73px)] bg-[#f4f6f8] px-4 py-8 text-zinc-950 sm:px-8">
      <div className="mx-auto max-w-2xl">
        <p className="text-xs font-black uppercase tracking-[0.18em] text-zinc-500">
          Gestão da equipe
        </p>
        <h1 className="mt-2 text-4xl font-black uppercase tracking-tight">
          Novo funcionário
        </h1>
        <p className="mt-3 text-lg font-medium text-zinc-600">
          O acesso será restrito ao incubatório do gerente.
        </p>
        <form
          onSubmit={submit}
          className="mt-8 rounded-[24px] border-2 border-zinc-950 bg-white p-5 shadow-[6px_6px_0_0_#18181b] sm:p-7"
        >
          <UserPlus className="mb-5 size-8" />
          <div className="grid gap-4 sm:grid-cols-2">
            {["nome", "cargo", "usuario", "senha"].map((name) => (
              <div key={name}>
                <Label
                  htmlFor={name}
                  className="mb-2 block text-xs font-black uppercase tracking-wide"
                >
                  {name === "usuario"
                    ? "Usuário"
                    : name[0].toUpperCase() + name.slice(1)}
                </Label>
                <Input
                  id={name}
                  name={name}
                  type={name === "senha" ? "password" : "text"}
                  value={form[name]}
                  onChange={change}
                  required
                  className="h-12 rounded-xl border-2 border-zinc-950 px-3"
                />
              </div>
            ))}
          </div>
          {message && (
            <p className="mt-4 text-sm font-bold text-emerald-700">{message}</p>
          )}
          <Button
            type="submit"
            disabled={loading}
            className="mt-6 h-12 w-full rounded-xl border-2 border-zinc-950 bg-[#f6c945] font-black uppercase text-zinc-950 shadow-[0_3px_0_0_#18181b]"
          >
            {loading ? "Cadastrando..." : "Criar acesso"}
          </Button>
        </form>
      </div>
    </main>
  );
}
