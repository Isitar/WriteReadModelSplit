# Write Read Model Split

> :warning: **This is just a demo project**


Demo project splitting read and write models.

Domain models are stored in postgresql db, update & create trigger a cache bust.

Read models are cached in inmemory cache (with sliding expiration).

## Benefits
Speed for data that doesn't change often

## Drawbacks

The current implementation will result in slower writes since all cache updating is done in-process.
A solution for this could be a deferred cache update or to simply bust the cache for the entity and reload it with the next query.

This can become very memory intensive

The current implementation does not differentiate between list entries and full models

## Project structure

Commands get the DomainContext injected and work directly with the domain models.

Queries will query the ids on the domain models and then get the vms from a separate repository

## Run it

- install something to run docker-compose
- install dotnet (.NET 6 at the time of writing) 

```bash
docker-compose up -d
dotnet run Api/Api.csproj
```