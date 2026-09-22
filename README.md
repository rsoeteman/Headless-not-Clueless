# Headless Not Clueless

This repository is the reference/companion project for the **"Headless Not Clueless"** talk by Richard Soeteman. It demonstrates how to use and extend Umbraco's **Content Delivery API**, including custom filters, custom sorting, and a custom Delivery API endpoint for dictionary translations.

## Project structure

```
Headless-not-Clueless/
├── HeadlessNotClueless.Umbraco/            # Main Umbraco web project
├── HeadlessNotClueless.UmbracoExtensions/  # Delivery API customizations
├── HeadlessNotClueless.UmbracoModels/      # ModelsBuilder generated models
└── Bruno/Headless not Clueless samples/    # Bruno API collection with sample requests
```

- **HeadlessNotClueless.Umbraco** — The main Umbraco web/backend project. Hosts the backoffice, the website, and the Delivery API (enabled in `Program.cs` via `.AddDeliveryApi()`).
- **HeadlessNotClueless.UmbracoExtensions** — Class library containing the Delivery API customizations for this demo:
  - `Filters/SiteFilter.cs` and `Filters/PublicationDateFilter.cs` — custom filter handlers adding the `website` and `publicationdate` filters to the content endpoint
  - `Indexers/` and `Sorters/PublicationDateSorter.cs` — supporting indexing/sorting for the custom filters
  - `Resolvers/CustomPathResolver.cs` — custom API content path resolution
  - `Controllers/TranslationAPIController.cs` — a custom Delivery API endpoint that exposes dictionary translations
- **HeadlessNotClueless.UmbracoModels** — ModelsBuilder-generated, strongly-typed content models.
- **Bruno/Headless not Clueless samples/** — A [Bruno](https://www.usebruno.com/) API collection with ready-to-run sample requests for the endpoints described below (News Overview, News Item, Translations), plus a `Local Development` environment.

## Getting started / login

The project uses a local SQLite database (`umbracoDbDSN` in `appsettings.Development.json`), so there's no external database server to install or configure — just clone the repo and run the `HeadlessNotClueless.Umbraco` project.

- Site URL: `https://localhost:44345`
- Backoffice URL: `https://localhost:44345/umbraco`
- Backoffice login:
  - Username: `testuser@soetemansoftware.nl`
  - Password: `test123456`

The Delivery API is **not** publicly accessible in this project (`DeliveryApi:PublicAccess` is `false`), so every request — whether from Swagger, Bruno, or a real frontend — needs an `Api-Key` header.

## Using Swagger to explore the Delivery API

With the Delivery API enabled, Umbraco exposes a Swagger UI at:

```
https://localhost:44345/umbraco/openapi/index.html
```

This lets you browse and try out the Content Delivery API endpoints directly from the browser, including the custom additions in this project — for example, the custom translations endpoint is grouped under **"Translations"** (via `[ApiExplorerSettings(GroupName = "Translations")]` on `TranslationAPIController`).

Because `PublicAccess` is `false`, you need to supply the `Api-Key` header when trying out requests in Swagger, the same way it's required for every request below.

### News Overview

```
GET /umbraco/delivery/api/v2/content
```

Query parameters:

| Parameter | Value | Notes |
|---|---|---|
| `filter` | `publicationdate` | custom filter from `PublicationDateFilter` |
| `filter` | `website:bd7bd3e5-3cca-464e-8526-43ee84d457ad` | custom filter from `SiteFilter`, scopes to a site (NL vs BE) — this is the `nl-id` |
| `skip` | `0` | paging |
| `take` | `10` | paging |
| `expand` | `properties[$all]` | expand all properties |
| `fields` | `properties[$all]` | include all properties in the response |
| `sort` | `publicationdate` | sort by the custom publication date field |

Headers:

| Header | Value |
|---|---|
| `Api-Key` | `keep-this-out-of-git` |
| `Start-Item` | `news` |

For BE content, use `website:ed7d6dc7-c453-44fd-b077-4b4a8e81b89f` (`be-id`) instead.

### News Item (single item)

```
GET /umbraco/delivery/api/v2/content/item/news/{url-segment}
```

Example:

```
GET /umbraco/delivery/api/v2/content/item/news/new-front-end-hire
```

Query parameters:

| Parameter | Value |
|---|---|
| `expand` | `properties[$all]` |
| `fields` | `properties[$all]` |

Headers:

| Header | Value |
|---|---|
| `Api-Key` | `keep-this-out-of-git` |
| `Start-Item` | `bd7bd3e5-3cca-464e-8526-43ee84d457ad` (`nl-id`) |

For BE content, use `Start-Item: ed7d6dc7-c453-44fd-b077-4b4a8e81b89f` (`be-id`) instead.

### Translations API

This is a custom Delivery API endpoint implemented by `TranslationAPIController`:

```
GET /umbraco/delivery/api/v2/translations
```

Query parameters:

| Parameter | Value | Notes |
|---|---|---|
| `locale` | `en` | required |
| `startitem` | `app` | optional — scopes the result to a dictionary subtree |

Headers:

| Header | Value |
|---|---|
| `Api-Key` | `keep-this-out-of-git` |

It returns a flat key/value dictionary of all non-empty dictionary translations for the given locale, scoped to the descendants of the `startitem` dictionary item (or the whole dictionary if `startitem` is omitted).

## Bruno

[Bruno](https://www.usebruno.com/) is a free, open-source API client (similar to Postman/Insomnia) that stores collections as plain text `.bru`/`.yml` files instead of a proprietary format, so they can live in this repo and be versioned alongside the code.

The full sample collection lives under `Bruno/Headless not Clueless samples/` and mirrors the endpoints documented above:

- `News Overview/` — `News Overview NL.yml` and `News Overview BE.yml`, for the content list endpoint
- `News Item/` — `News Item from NL.yml` and `News Item from BE.yml`, for the single-item endpoint
- `Translations/` — `App Translations.yml`, `All Translations.yml`, and `Website Translations.yml`, showing different `startitem` scopes

To use it:

1. Install [Bruno](https://www.usebruno.com/) and open it.
2. Choose **Open Collection** and point it at `Bruno/Headless not Clueless samples/`.
3. Select the **Local Development** environment (top-right environment selector). It defines the `host`, `be-id` (`ed7d6dc7-c453-44fd-b077-4b4a8e81b89f`), `nl-id` (`bd7bd3e5-3cca-464e-8526-43ee84d457ad`), and a secret `Api-Key` variable.
4. Set the `Api-Key` environment variable to the value of `DeliveryApi:ApiKey` in `appsettings.json` (`keep-this-out-of-git` in this project).
5. Open any request in the collection and hit **Send** — the request uses the environment variables above, so no further editing is needed.

## Further reading

- [Content Delivery API (official docs)](https://docs.umbraco.com/umbraco-cms/develop-with-umbraco/headless-and-apis/content-delivery-api)
- [Delivery API: tip, trick and hack (old but still gold)](https://kjac.dev/posts/delivery-api-tip-trick-and-hack/)
- [Quick tip: extending the Delivery API's content response](https://www.johanreitsma.com/blogs/quick-tip-extending-the-delivery-apis-content-response/)
- [Automatically filter content Delivery API content](https://richardsoeteman.net/blog/automatically-filter-content-delivery-api-content/)

## Author

Richard Soeteman

- [soetemansoftware.nl](https://soetemansoftware.nl)
- [richardsoeteman.net](https://richardsoeteman.net)
- [LinkedIn](https://www.linkedin.com/in/richardsoeteman/)
