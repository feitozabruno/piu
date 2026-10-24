import { db, registrarUltimaSincronizacao } from "./database";
import { apiFetch } from "@/shared/lib/api";

let sincronizacaoEmAndamento = false;

export async function sincronizarRecebimentos() {
  if (sincronizacaoEmAndamento || !navigator.onLine) return;

  sincronizacaoEmAndamento = true;

  try {
    const pendentes = await db.recebimentos
      .where("status")
      .equals("pending")
      .sortBy("criadoEm");

    for (const recebimento of pendentes) {
      await sincronizarRecebimento(recebimento);
    }
  } finally {
    sincronizacaoEmAndamento = false;
  }
}

async function sincronizarRecebimento(recebimento) {
  await db.recebimentos.update(recebimento.clientId, {
    status: "syncing",
  });

  try {
    const resposta = await apiFetch("/ovos", {
      method: "POST",
      body: JSON.stringify({
        clientId: recebimento.clientId,
        lote: recebimento.lote,
        dataPostura: recebimento.dataPostura,
        ninhoIncubavel: recebimento.ninhoIncubavel,
        camaIncubavel: recebimento.camaIncubavel,
        temperaturaOvo: recebimento.temperaturaOvo,
        temperaturaBau: recebimento.temperaturaBau,
        placaCaminhao: recebimento.placaCaminhao,
        motorista: recebimento.motorista,
        horaRecebimento: recebimento.horaRecebimento,
        observacao: recebimento.observacao,
      }),
    });

    await db.recebimentos.update(recebimento.clientId, {
      status: "synced",
      serverId: resposta.id,
      sincronizadoEm: new Date().toISOString(),
      ultimoErro: null,
    });
    await registrarUltimaSincronizacao();
  } catch (error) {
    const deveFalhar =
      error.status >= 400 && error.status < 500 && error.status !== 401;

    await db.recebimentos.update(recebimento.clientId, {
      status: deveFalhar ? "failed" : "pending",
      tentativas: recebimento.tentativas + 1,
      ultimoErro: error.message,
    });
  }
}

export function iniciarSincronizacao() {
  const sincronizar = () => void sincronizarRecebimentos();

  window.addEventListener("online", sincronizar);
  const intervalo = window.setInterval(sincronizar, 30_000);
  sincronizar();

  return () => {
    window.removeEventListener("online", sincronizar);
    window.clearInterval(intervalo);
  };
}
