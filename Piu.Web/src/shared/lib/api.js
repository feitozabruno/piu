const BASE_URL = import.meta.env.VITE_API_URL ?? "/api/v1";

export async function apiFetch(endpoint, options = {}) {
  let response;
  const url = `${BASE_URL.replace(/\/$/, "")}${endpoint}`;

  try {
    response = await fetch(url, {
      ...options,
      credentials: "include",
      headers: {
        "Content-Type": "application/json",
        ...options.headers,
      },
    });
  } catch (error) {
    const networkError = new Error("Sem conexão com a API.");
    networkError.isNetworkError = true;
    networkError.cause = error;
    throw networkError;
  }

  if (!response.ok) {
    const errorData = await response.json().catch(() => null);

    if (response.status === 401 && !endpoint.startsWith("/auth/")) {
      window.dispatchEvent(new CustomEvent("auth:unauthorized"));
    }

    const error = new Error(
      errorData?.detail ??
        errorData?.title ??
        errorData?.message ??
        "Erro na requisição",
    );
    error.status = response.status;
    error.data = errorData;
    throw error;
  }

  const responseText = await response.text();

  if (!responseText.trim()) return null;

  return JSON.parse(responseText);
}
