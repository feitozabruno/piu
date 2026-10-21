import Dexie from "dexie";

export const db = new Dexie("piu-offline");

db.version(1).stores({
  recebimentos: "clientId, status, criadoEm, lote",
  lotes: "id, nome",
});

db.version(2).stores({
  recebimentos: "clientId, status, criadoEm, lote",
  lotes: "id, nome",
  configuracao: "chave",
});

export async function enfileirarRecebimento(payload) {
  const clientId = crypto.randomUUID();

  await db.recebimentos.add({
    clientId,
    ...payload,
    status: "pending",
    tentativas: 0,
    criadoEm: new Date().toISOString(),
    ultimoErro: null,
    serverId: null,
  });

  return clientId;
}

export async function registrarUltimaSincronizacao() {
  await db.configuracao.put({
    chave: "ultimaSincronizacao",
    valor: new Date().toISOString(),
  });
}
