"use client";

import { useQuery } from "@tanstack/react-query";
import { SystemApi } from "@/api";

export default function SettingsPage() {
  const { data: system } = useQuery({ queryKey: ["system"], queryFn: SystemApi.get });

  const rows: [string, string][] = system
    ? [
        ["Edition", system.edition],
        ["Results", system.mock ? "Placeholders (AI_USE_MOCK=true)" : "Real AI"],
        ["Anthropic key", system.anthropicKey ? "Set" : "Not set"],
      ]
    : [];

  return (
    <div className="mx-auto max-w-2xl">
      <h1 className="font-display text-2xl font-semibold">Settings</h1>
      <dl className="mt-6 divide-y divide-border rounded-lg border border-border bg-card">
        {rows.map(([label, value]) => (
          <div key={label} className="flex justify-between gap-4 px-5 py-3 text-sm">
            <dt className="text-muted-foreground">{label}</dt>
            <dd className="font-medium">{value}</dd>
          </div>
        ))}
      </dl>
      <p className="mt-4 text-sm text-muted-foreground">Keys are set in .env on the server, never here.</p>
    </div>
  );
}
