import { useState } from "react";

const BASE_URL = "https://piu-vhfi.onrender.com/api/v1";

export async function apiFetch(endpoint, options = {}) {
  const response = await fetch(`${BASE_URL}${endpoint}`, {
    ...options,
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
      ...options.headers,
    },
  });

  if (!response.ok) {
    const errorData = await response.json().catch(() => null);
    throw errorData ?? new Error("Erro na requisição");
  }

  if (response.status === 204) return null;

  return response;
  // return response.json();
}

export function useLogin() {
  const [formData, setFormData] = useState({
    usuario: "",
    senha: "",
  });

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [showPassword, setShowPassword] = useState(false);

  const handleChange = (e) => {
    if (error) {
      setError(null);
    }

    const { id, value } = e.target;
    setFormData((prevData) => ({
      ...prevData,
      [id]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);

    try {
      await apiFetch("/auth/entrar", {
        method: "POST",
        body: JSON.stringify({
          usuario: formData.usuario,
          senha: formData.senha,
        }),
      });
      return true;
    } catch (err) {
      setError(err.message);
      return false;
    } finally {
      setIsLoading(false);
    }
  };

  const toggleShowPassword = () => {
    setShowPassword((prev) => !prev);
  };

  return {
    formData,
    isLoading,
    error,
    showPassword,
    handleChange,
    handleSubmit,
    toggleShowPassword,
  };
}
