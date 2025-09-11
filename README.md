# Url Shortener

A URL shortening system built with ASP.NET Core, using Clean Architecture and CQRS.

## 🚀 Features
- Shorten long URLs into short codes
- Track clicks with analytics
- Authentication & JWT refresh tokens
- Caching (Cache-Aside pattern)
- Domain events for analytics
- System design includes sharding & consistent hashing (documented, not fully implemented)

## 📐 System Design
![System Design](images/UrlShortener_HLD.drawio)

Includes:
- Capacity estimation
- High-level architecture
- Sequence diagrams
- Collision handling
- Caching strategy

## 📊 API Endpoints
- **Authentication**
  - `POST /api/auth/register`
  - `POST /api/auth/login`
  - `POST /api/auth/logout`
- **Analytics**
  - `GET /api/Analytics/summary`
  - `GET /api/Analytics/top-urls`
- **Urls**
  - `POST /api/Urls/shorten`
  - `GET /api/Urls/mine` (paginated user URLs)

## 🛠️ Tech Stack
- ASP.NET Core (.NET 8)
- Entity Framework Core
- MediatR (CQRS)
- PostgreSQL
- Redis (caching)
