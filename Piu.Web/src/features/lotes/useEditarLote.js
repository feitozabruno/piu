import { useState } from "react";
import { apiFetch } from "@/shared/lib/api";

export function useEditarLote(carregarLotes) {
  const [lote, setLote] = useState(null);
  const [nome, setNome] = useState("");
  const [dataNascimento, setDataNascimento] = useState("");
  const [ativo, setAtivo] = useState(true);
  const [erro, setErro] = useState(null);
  const [salvando, setSalvando] = useState(false);

  function iniciar(loteSelecionado) {
    setLote(loteSelecionado);
    setNome(loteSelecionado.nome);
    setDataNascimento(loteSelecionado.dataNascimento);
    setAtivo(loteSelecionado.ativo);
    setErro(null);
  }

  function fechar() {
    if (salvando) return;
    setLote(null);
    setErro(null);
  }

  async function submeter(event) {
    event.preventDefault();
    setErro(null);

    const nomeNormalizado = nome.trim();
    const hoje = new Date().toISOString().slice(0, 10);

    if (nomeNormalizado.length < 3 || nomeNormalizado.length > 30) {
      setErro("O nome deve ter entre 3 e 30 caracteres.");
      return;
    }

    if (!dataNascimento) {
      setErro("Informe a data de nascimento.");
      return;
    }

    if (dataNascimento > hoje) {
      setErro("A data de nascimento não pode estar no futuro.");
      return;
    }

    setSalvando(true);

    try {
      await apiFetch(`/lotes/${lote.id}`, {
        method: "PUT",
        body: JSON.stringify({
          nome: nomeNormalizado,
          dataNascimento,
          ativo,
        }),
      });

      await carregarLotes();
      setLote(null);
    } catch (error) {
      setErro(
        error instanceof Error
          ? error.message
          : "Não foi possível atualizar o lote.",
      );
    } finally {
      setSalvando(false);
    }
  }

  return {
    aberto: Boolean(lote),
    ativo,
    dataNascimento,
    erro,
    fechar,
    iniciar,
    nome,
    salvando,
    setAtivo,
    setDataNascimento,
    setNome,
    submeter,
  };
}
