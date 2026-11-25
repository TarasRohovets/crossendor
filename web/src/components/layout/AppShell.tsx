"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { NAV } from "@/edition/nav";
import { cn } from "@/lib/utils";

export function AppShell({ children }: { children: React.ReactNode }) {
  const pathname = usePathname();
  return (
    <div className="flex min-h-screen flex-col bg-canvas md:flex-row">
      <aside className="flex shrink-0 gap-1 overflow-x-auto border-b border-border bg-background p-3 md:w-60 md:flex-col md:border-b-0 md:border-r md:p-4">
        <Link href="/create" className="mr-3 flex items-center px-2 py-2 font-display text-lg font-semibold md:mb-6 md:mr-0">
          <span className="bg-gradient-to-r from-ember-a to-ember-b bg-clip-text text-transparent">Crossendor</span>
        </Link>
        {NAV.map((item) => {
          const active = pathname === item.href || pathname.startsWith(`${item.href}/`);
          return (
            <Link
              key={item.href}
              href={item.href}
              className={cn(
                "flex items-center gap-2.5 whitespace-nowrap rounded-md px-3 py-2 text-sm font-medium transition-colors",
                active ? "bg-primary/10 text-primary" : "text-muted-foreground hover:bg-muted hover:text-foreground",
              )}
            >
              <item.icon className="h-4 w-4" />
              {item.label}
            </Link>
          );
        })}
      </aside>
      <main className="min-w-0 flex-1 p-4 md:p-10">{children}</main>
    </div>
  );
}
