import type { ShopStatus } from "@/models";
import { cn } from "@/lib/utils";

const LABELS: Record<ShopStatus, string> = {
  writing: "Writing",
  draft: "Draft",
  exported: "Exported",
  failed: "Failed",
};

const TONES: Record<ShopStatus, string> = {
  writing: "bg-primary/10 text-primary",
  draft: "bg-primary/10 text-primary",
  exported: "bg-volt/15 text-volt",
  failed: "bg-destructive/10 text-destructive",
};

export function StatusChip({ status }: { status: ShopStatus }) {
  return (
    <span className={cn("shrink-0 rounded-full px-2.5 py-0.5 text-xs font-semibold", TONES[status])}>{LABELS[status]}</span>
  );
}
