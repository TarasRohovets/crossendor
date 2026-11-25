"use client";

import { useParams } from "next/navigation";
import { useQuery } from "@tanstack/react-query";
import { Download } from "lucide-react";
import { ShopsApi } from "@/api";
import { Button } from "@/components/ui/button";
import { StatusChip } from "@/components/shops/StatusChip";
import { hostOf } from "@/lib/utils";

export default function ShopPage() {
  const { id } = useParams<{ id: string }>();
  const { data: shop, isLoading } = useQuery({ queryKey: ["shops", id], queryFn: () => ShopsApi.get(id) });

  if (isLoading) return <p className="text-sm text-muted-foreground">Loading…</p>;
  if (!shop) return <p className="text-sm text-muted-foreground">This shop doesn't exist.</p>;

  const content = shop.content;
  return (
    <div className="mx-auto max-w-3xl">
      <div className="flex flex-wrap items-center gap-3">
        <h1 className="font-display text-2xl font-semibold">{shop.name}</h1>
        <StatusChip status={shop.status} />
      </div>
      <p className="mt-1 text-sm text-muted-foreground">From {hostOf(shop.sourceUrl)}</p>

      {shop.error && <p className="mt-6 rounded-md bg-destructive/10 p-4 text-sm text-destructive">{shop.error}</p>}

      {content && (
        <div className="mt-8 space-y-6">
          <section className="rounded-lg border border-border bg-card p-6">
            <p className="text-sm text-muted-foreground">{content.tagline}</p>
            <h2 className="mt-2 font-display text-xl font-semibold">{content.homepageHeadline}</h2>
            <p className="mt-3 text-sm leading-relaxed">{content.productDescription}</p>
          </section>
          <section className="rounded-lg border border-border bg-card p-6">
            <h2 className="font-semibold">FAQ</h2>
            <dl className="mt-3 space-y-3 text-sm">
              {content.faq.map((item) => (
                <div key={item.question}>
                  <dt className="font-medium">{item.question}</dt>
                  <dd className="text-muted-foreground">{item.answer}</dd>
                </div>
              ))}
            </dl>
          </section>
          <div className="flex flex-wrap items-center gap-3">
            <Button variant="outline" disabled>
              <Download className="h-4 w-4" />
              Download theme and products
            </Button>
            <span className="text-sm text-muted-foreground">Export to Shopify comes next.</span>
          </div>
        </div>
      )}
    </div>
  );
}
