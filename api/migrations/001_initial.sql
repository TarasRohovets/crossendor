-- Crossendor store builder. Everything lives in its own schema, so it can share a database with other apps
-- (Crossendor Cloud runs these files against the existing Crossendor database).
-- Every file here runs at startup in name order and must be safe to run twice.

CREATE SCHEMA IF NOT EXISTS builder;

CREATE TABLE IF NOT EXISTS builder.shops (
    id          uuid PRIMARY KEY,
    owner_id    text NOT NULL,           -- "local" here; the account id in the cloud
    status      text NOT NULL,           -- Writing | Draft | Exported | Failed
    source_url  text NOT NULL,
    data        jsonb NOT NULL,          -- the whole shop: product facts, store content
    created_at  timestamptz NOT NULL DEFAULT now(),
    updated_at  timestamptz NOT NULL DEFAULT now()
);
CREATE INDEX IF NOT EXISTS shops_owner_created ON builder.shops (owner_id, created_at DESC);

CREATE TABLE IF NOT EXISTS builder.shop_exports (
    id          uuid PRIMARY KEY,
    shop_id     uuid NOT NULL REFERENCES builder.shops (id) ON DELETE CASCADE,
    kind        text NOT NULL,           -- theme-zip | shopify
    status      text NOT NULL,
    data        jsonb NOT NULL,
    created_at  timestamptz NOT NULL DEFAULT now()
);
CREATE INDEX IF NOT EXISTS shop_exports_shop ON builder.shop_exports (shop_id, created_at DESC);
