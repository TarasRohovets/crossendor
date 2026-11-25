// Mirrors api/Domain/Shops.cs.

export type ShopStatus = "writing" | "draft" | "exported" | "failed";

export interface ProductFacts {
  title: string;
  description?: string;
  price?: number;
  currency?: string;
  images: string[];
  variants: string[];
  sourceUrl: string;
}

export interface FaqItem {
  question: string;
  answer: string;
}

export interface StoreContent {
  brandName: string;
  tagline: string;
  homepageHeadline: string;
  productDescription: string;
  faq: FaqItem[];
  about: string;
}

export interface Shop {
  id: string;
  sourceUrl: string;
  name: string;
  status: ShopStatus;
  product?: ProductFacts;
  content?: StoreContent;
  error?: string;
  createdAt: string;
  updatedAt: string;
}

export interface SystemInfo {
  edition: string;
  mock: boolean;
  anthropicKey: boolean;
}
