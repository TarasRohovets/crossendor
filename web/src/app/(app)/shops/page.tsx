"use client";

import Link from "next/link";
import { useQuery } from "@tanstack/react-query";
import { ShopsApi } from "@/api";
import { Button } from "@/components/ui/button";
import { StatusChip } from "@/components/shops/StatusChip";
import { hostOf } from "@/lib/utils";

export default function ShopsPage() {
  const { data: shops, isLoading } = useQuery({ queryKey: ["shops"], queryFn: ShopsApi.list });

  return (
    <div className="mx-auto max-w-5xl">
      <h1 className="font-display text-2xl font-semibold">My shops</h1>
      {isLoading ? (
        <p className="mt-6 text-sm text-muted-foreground">Loading…</p>
      ) : !shops?.length ? (
        <div className="mt-10 rounded-lg border border-dashed border-border p-10 text-center">
          <p className="text-muted-foreground">No shops yet. Paste a product link to start one.</p>
          <Link href="/create" className="mt-5 inline-block">
            <Button>Create shop</Button>
          </Link>
        </div>
      ) : (
        <ul className="mt-6 grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
          {shops.map((shop) => (
            <li key={shop.id}>
              <Link
                href={`/shops/${shop.id}`}
                className="block rounded-lg border border-border bg-card p-5 transition hover:border-primary/50"
              >
                <div className="flex items-start justify-between gap-3">
                  <span className="min-w-0 truncate font-semibold">{shop.name}</span>
                  <StatusChip status={shop.status} />
                </div>
                <p className="mt-2 truncate text-sm text-muted-foreground">{hostOf(shop.sourceUrl)}</p>
              </Link>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}
