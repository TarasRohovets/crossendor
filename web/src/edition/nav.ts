import { Settings, Store, Wand2, type LucideIcon } from "lucide-react";

/**
 * The sidebar. This file belongs to the edition: Crossendor Cloud replaces it (adding Ads and the account),
 * so the shared shell never needs editing for one edition alone.
 */
export type NavItem = { href: string; label: string; icon: LucideIcon };

export const EDITION = "open-source";

export const NAV: NavItem[] = [
  { href: "/create", label: "Create shop", icon: Wand2 },
  { href: "/shops", label: "My shops", icon: Store },
  { href: "/settings", label: "Settings", icon: Settings },
];
