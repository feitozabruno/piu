import { useEffect, useState } from "react";
import { Building2, UserPlus } from "lucide-react";
import { apiFetch } from "@/shared/lib/api";
import { Button } from "@/shared/components/ui/button";
import { Input } from "@/shared/components/ui/input";
import { Label } from "@/shared/components/ui/label";

const initialForm = {
  identificador: "",
  nome: "",
  sigla: "",
  usuarioGerente: "",
  senhaGerente: "",
  nomeGerente: "",
};

export function PainelAdministracao() {
  const [incubatorios, setIncubatorios] = useState([]);
  const [form, setForm] = useState(initialForm);
  const [message, setMessage] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    let active = true;
    apiFetch("/admin/incubatorios")
      .then((data) => {
        if (active) setIncubatorios(data);
      })
      .catch((error) => {
        if (active) {
          setMessage({ type: "error", text: error.message });
        }
      });

    return () => {
      active = false;
    };
  }, []);

  function handleChange(event) {
    setForm((current) => ({ ...current, [event.target.name]: event.target.value }));
    setMessage(null);
  }

  async function handleSubmit(event) {
    event.preventDefault();
    setLoading(true);
    setMessage(null);
    try {
      await apiFetch("/admin/incubatorios", {
        method: "POST",
        body: JSON.stringify(form),
      });
      setForm(initialForm);
      setMessage({ type: "success", text: "Incubatório e gerente cadastrados." });
      const incubatoriosAtualizados = await apiFetch("/admin/incubatorios");
      setIncubatorios(incubatoriosAtualizados);
    } catch (error) {
      setMessage({ type: "error", text: error.message });
    } finally {
      setLoading(false);
    }
  }

  return (
    <main className="min-h-[calc(100vh-73px)] bg-[#f4f6f8] px-4 py-8 text-zinc-950 sm:px-8 lg:px-10">
      <div className="mx-auto max-w-7xl">
        <p className="text-xs font-black uppercase tracking-[0.18em] text-zinc-500">Controle do sistema</p>
        <h1 className="mt-2 text-4xl font-black uppercase tracking-tight sm:text-5xl">Administração</h1>
        <p className="mt-3 max-w-2xl text-lg font-medium text-zinc-600">Cadastre incubatórios e conceda o primeiro acesso ao gerente responsável.</p>

        <div className="mt-8 grid gap-6 lg:grid-cols-[minmax(0,1fr)_minmax(0,1.15fr)]">
          <form onSubmit={handleSubmit} className="rounded-[24px] border-2 border-zinc-950 bg-white p-5 shadow-[6px_6px_0_0_#18181b] sm:p-7">
            <div className="mb-6 flex items-center gap-3">
              <div className="flex size-12 items-center justify-center rounded-xl border-2 border-zinc-950 bg-[#f6c945]"><Building2 /></div>
              <h2 className="text-2xl font-black uppercase">Novo incubatório</h2>
            </div>
            <div className="grid gap-4 sm:grid-cols-2">
              <Field label="Identificador" name="identificador" value={form.identificador} onChange={handleChange} required />
              <Field label="Sigla" name="sigla" value={form.sigla} onChange={handleChange} required />
              <Field label="Nome do incubatório" name="nome" value={form.nome} onChange={handleChange} required wide />
              <Field label="Nome do gerente" name="nomeGerente" value={form.nomeGerente} onChange={handleChange} required wide />
              <Field label="Usuário do gerente" name="usuarioGerente" value={form.usuarioGerente} onChange={handleChange} required />
              <Field label="Senha provisória" name="senhaGerente" type="password" value={form.senhaGerente} onChange={handleChange} required />
            </div>
            {message && <p className={`mt-4 text-sm font-bold ${message.type === "error" ? "text-red-600" : "text-emerald-700"}`}>{message.text}</p>}
            <Button
              type="submit"
              disabled={loading}
              className="mt-6 h-12 w-full rounded-xl border-2 border-zinc-950 bg-[#f6c945] font-black uppercase text-zinc-950 shadow-[0_3px_0_0_#18181b]"
            >
              {loading ? "Cadastrando..." : "Cadastrar incubatório"}
            </Button>
          </form>

          <section className="rounded-[24px] border-2 border-zinc-950 bg-white p-5 sm:p-7">
            <div className="mb-6 flex items-center gap-3"><UserPlus className="size-7" /><h2 className="text-2xl font-black uppercase">Incubatórios cadastrados</h2></div>
            <div className="grid gap-3">
              {incubatorios.map((item) => <article key={item.identificador} className="rounded-2xl border-2 border-zinc-200 p-4"><div className="flex items-start justify-between gap-3"><div><p className="text-lg font-black">{item.nome}</p><p className="text-sm font-bold text-zinc-500">{item.sigla} · {item.identificador}</p></div><span className="rounded-full bg-emerald-100 px-3 py-1 text-xs font-black uppercase text-emerald-700">{item.ativo ? "Ativo" : "Inativo"}</span></div><p className="mt-3 text-sm font-medium text-zinc-600">{item.gerente ? `Gerente: ${item.gerente.nome} (@${item.gerente.userName})` : "Sem gerente vinculado"}</p></article>)}
              {incubatorios.length === 0 && <p className="rounded-xl bg-zinc-100 p-4 text-sm font-bold text-zinc-600">Nenhum incubatório cadastrado.</p>}
            </div>
          </section>
        </div>
      </div>
    </main>
  );
}

function Field({ label, name, wide, ...props }) {
  return <div className={wide ? "sm:col-span-2" : ""}><Label htmlFor={name} className="mb-2 block text-xs font-black uppercase tracking-wide text-zinc-700">{label}</Label><Input id={name} name={name} {...props} className="h-12 rounded-xl border-2 border-zinc-950 px-3" /></div>;
}
