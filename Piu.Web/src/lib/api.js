const BASE_URL = "/api/v1";

export async function apiFetch(endpoint, options = {}) {
  let response;

  try {
    response = await fetch(`${BASE_URL}${endpoint}`, {
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

  if (response.status === 204) return null;

  return response.json();
}
