# Crossendor

**Turn a product link into a Shopify store.** Open source, on your own Claude key.

Paste a link to a product page. Crossendor reads the product, writes the store around it (name, homepage,
product page, FAQ, About, policies) and hands it to you as a Shopify theme plus a product file to upload, or
pushes it straight into your Shopify store.

> **Status: early.** This is the skeleton. Today a link goes through placeholder steps end to end, so the app
> runs and the flow can be clicked through. The real product reader, store writer and Shopify export come next
> (see [Roadmap](#roadmap)).

## Quick start

You need [Docker](https://docs.docker.com/get-docker/).

```bash
git clone https://github.com/TarasRohovets/crossendor.git
cd crossendor
cp .env.example .env        # set POSTGRES_PASSWORD to any long random string
docker compose up -d
```

Open **http://localhost:3000**, paste a product link and press **Create shop**.

## How it's built

```
web/   Next.js app: the UI. Proxies /api to the API, so the browser talks to one origin.
api/   .NET 9 API: product reading, store writing, export. PostgreSQL via Dapper.
```

- **Database:** PostgreSQL. The schema is plain SQL in `api/migrations/`, applied on startup, all in its own
  `builder` schema.
- **Pipeline:** three steps behind interfaces in `api/Services/Pipeline.cs`: read the product
  (`IProductImporter`), write the store (`IStoreWriter`), export it (`IShopExporter`).
- **Editions:** `api/Edition.cs` and `web/src/edition/` hold the few parts that differ between this open-source
  edition (one local user, no sign-in) and the hosted Crossendor Cloud. Everything else is shared.

## Roadmap

1. **Product from a link:** Shopify product pages first, then AliExpress and Amazon.
2. **Store writer:** Claude writes the brand, the homepage sections, the product page, the FAQ, the About page and the policies.
3. **Base theme:** one Online Store 2.0 theme the writer fills in.
4. **Export:** download a theme ZIP and a Shopify product CSV, or connect a store with a custom app token and push it.

## Development

Run the database in Docker and the two apps on your machine:

```bash
docker compose -f docker-compose.yml -f compose.dev.yml up -d db

cd api
export ConnectionStrings__PostgresConnection="Host=localhost;Database=crossendor;Username=crossendor;Password=$POSTGRES_PASSWORD"
dotnet run                             # http://localhost:5310, Swagger at /swagger

cd web
npm install
npm run dev                            # http://localhost:3000
```

To change the schema, add the next file to `api/migrations/` (for example `002_add_x.sql`). Every file runs at
startup in name order, so each must be safe to run twice (`IF NOT EXISTS`).

## Security

Crossendor has **no accounts and no sign-in**. It only answers on the computer it runs on
(`BIND_ADDRESS=127.0.0.1`). Don't put it on the public internet as it is: anyone who can open it can spend your AI key.

## Contributing

Issues and pull requests are welcome. For anything larger than a fix, open an issue first.

By contributing you agree that your contribution is licensed under AGPL-3.0 and may also be included in the
hosted edition of Crossendor.

## License

[AGPL-3.0](LICENSE). Copyright (c) 2026 Taras Rohovets.
