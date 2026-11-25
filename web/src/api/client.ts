import axios from "axios";

// Same origin: next.config.mjs proxies /api to the API.
export const http = axios.create({ baseURL: "/api", timeout: 300_000 });

/** The API's own message ({ error }) when it sent one. */
export function apiError(error: unknown, fallback = "Something went wrong. Try again.") {
  if (axios.isAxiosError(error)) return (error.response?.data as { error?: string } | undefined)?.error ?? fallback;
  return fallback;
}
