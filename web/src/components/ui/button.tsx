import { forwardRef } from "react";
import { cn } from "@/lib/utils";

type Variant = "primary" | "outline";

const styles: Record<Variant, string> = {
  // The one action on a page: the ember gradient.
  primary: "bg-gradient-to-r from-ember-a to-ember-b text-white shadow-sm hover:brightness-110",
  // Everything else: an outline in the accent, never gray.
  outline: "border border-primary/40 text-primary hover:bg-primary/10",
};

export const Button = forwardRef<HTMLButtonElement, React.ButtonHTMLAttributes<HTMLButtonElement> & { variant?: Variant }>(
  ({ className, variant = "primary", ...props }, ref) => (
    <button
      ref={ref}
      className={cn(
        "inline-flex h-11 items-center justify-center gap-2 rounded-md px-5 text-sm font-semibold transition focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50",
        styles[variant],
        className,
      )}
      {...props}
    />
  ),
);
Button.displayName = "Button";
