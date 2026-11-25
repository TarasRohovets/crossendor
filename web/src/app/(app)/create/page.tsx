"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import { Loader2, Wand2 } from "lucide-react";
import { toast } from "sonner";
import { apiError, ShopsApi } from "@/api";
import { Button } from "@/components/ui/button";

export default function CreateShopPage() {
  const router = useRouter();
  const queryClient = useQueryClient();
  const [url, setUrl] = useState("");

  const create = useMutation({
    mutationFn: () => ShopsApi.create(url.trim()),
    onSuccess: (shop) => {
      queryClient.invalidateQueries({ queryKey: ["shops"] });
      router.push(`/shops/${shop.id}`);
    },
    onError: (error) => toast.error(apiError(error)),
  });

  return (
    <div className="mx-auto max-w-2xl pt-6 md:pt-16">
      <h1 className="font-display text-3xl font-semibold leading-tight md:text-4xl">Paste a product link</h1>
      <p className="mt-3 text-muted-foreground">
        We read the product and write a store around it: name, homepage, product page, FAQ and policies.
      </p>
      <form
        className="mt-8 flex flex-col gap-3 sm:flex-row"
        onSubmit={(event) => {
          event.preventDefault();
          if (url.trim()) create.mutate();
        }}
      >
        <label htmlFor="product-url" className="sr-only">Product link</label>
        <input
          id="product-url"
          type="url"
          required
          value={url}
          onChange={(event) => setUrl(event.target.value)}
          placeholder="https://store.com/products/…"
          className="h-11 min-w-0 flex-1 rounded-md border border-input bg-card px-4 text-sm outline-none focus-visible:ring-2 focus-visible:ring-ring"
        />
        <Button type="submit" disabled={create.isPending}>
          {create.isPending ? <Loader2 className="h-4 w-4 animate-spin" /> : <Wand2 className="h-4 w-4" />}
          {create.isPending ? "Creating shop…" : "Create shop"}
        </Button>
      </form>
    </div>
  );
}
