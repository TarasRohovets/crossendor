import type { Shop, SystemInfo } from "@/models";
import { http } from "./client";

export { apiError } from "./client";

export const ShopsApi = {
  list: () => http.get<Shop[]>("/shops").then((r) => r.data),
  get: (id: string) => http.get<Shop>(`/shops/${id}`).then((r) => r.data),
  create: (url: string, angle?: string) => http.post<Shop>("/shops", { url, angle }).then((r) => r.data),
};

export const SystemApi = {
  get: () => http.get<SystemInfo>("/system").then((r) => r.data),
};
