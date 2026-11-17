# Equitech: Interview Preparation & Learning Module
> Part of the **Job Search & Career Development Agent** Platform (Implementation Blueprint v1.0)

This repository contains the standalone **Interview Preparation & Learning Module**, decoupled into an **ASP.NET Core 8 Web API** backend, a **Vue 3 + TypeScript** frontend, and an isolated **Standalone AI Agent Module**, with all credentials managed strictly through `.env` environment files.

---

## 1. REST API Specification (Blueprint Section 8)

All endpoints reside under `/api/v1`. The API implements:
- **ProblemDetails (RFC 7807)**: Structured error bodies with `errorCode`, `correlationId`, and timestamps.
- **Correlation IDs**: Header `X-Correlation-ID` generated or echoed on every request and response.
- **Cursor Pagination**: Query parameters `cursor` and `limit` returning `next_cursor` and `has_more`.
- **Server-Side Validation**: Strict schema and constraint validation.

### Complete API Endpoint Matrix

| Method | Endpoint | Purpose | Access Control | Status |
|---|---|---|---|---|
| `POST` | `/api/v1/cvs` | Create upload session & return short-lived upload URL | Authenticated candidate | ✅ Ready |
| `POST` | `/api/v1/cvs/{id}/process` | Start CV parsing & OCR extraction workflow | Owner only | ✅ Ready |
| `GET` | `/api/v1/profile` | Read confirmed candidate profile & search preferences | Owner only | ✅ Ready |
| `PATCH` | `/api/v1/profile` | Update candidate profile & search preferences | Owner only | ✅ Ready |
| `POST` | `/api/v1/job-searches` | Create job search workflow using profile/preferences | Owner only; quota limited | ✅ Ready |
| `GET` | `/api/v1/jobs` | List/filter ranked jobs with pagination cursor | Authenticated; user-scoped | ✅ Ready |
| `GET` | `/api/v1/jobs/{id}` | Job detail, source evidence, freshness, & match rationale | Authenticated | ✅ Ready |
| `POST` | `/api/v1/jobs/{id}/save` | Save or dismiss a job | Owner only | ✅ Ready |
| `POST` | `/api/v1/applications` | Create application tracker record | Owner only | ✅ Ready |
| `POST` | `/api/v1/applications/{id}/artifacts` | Generate tailored application materials & content hash | Owner only | ✅ Ready |
| `POST` | `/api/v1/approvals` | Create explicit approval for a specific action/content hash | Owner only; step-up auth | ✅ Ready |
| `POST` | `/api/v1/applications/{id}/submit` | Submit via supported integration or official-site handoff | Requires valid unconsumed approval | ✅ Ready |
| `GET` | `/api/v1/applications` | List pipeline and scheduled next actions | Owner only | ✅ Ready |
| `POST` | `/api/v1/interview-packs` | Generate interview preparation pack for selected job | Owner only | ✅ Ready |
| `POST` | `/api/v1/practice-sessions` | Start mock interview or evaluate practical exercise | Owner only | ✅ Ready |
| `POST` | `/api/v1/learning-plans` | Generate role-specific learning plan | Owner only | ✅ Ready |
| `GET` | `/api/v1/notifications` | List reminders and alerts | Owner only | ✅ Ready |
| `DELETE` | `/api/v1/account` | Request account and personal-data deletion | Owner only; confirmation required | ✅ Ready |
| `GET` | `/api/v1/admin/source-health` | Connector health and source operational statistics | Admin role | ✅ Ready |

---

## 2. Environment & Credentials Configuration (.env)

All credentials and sensitive configuration (Supabase connection string, OpenAI keys, API endpoints, CORS origins) are loaded strictly through `.env` files.

### 2.1 Backend Configuration (`backend/.env`)
```env
# Supabase Project Details
SUPABASE_URL=https://cskwxvcynuydovzdpros.supabase.co
SUPABASE_KEY=sb_publishable_Iuj0mPrfPdUxkqH05IskqA_7-1-ITN2

# Direct PostgreSQL Connection String for EF Core / Npgsql
SUPABASE_CONNECTION_STRING=Host=db.cskwxvcynuydovzdpros.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=YOUR_DATABASE_PASSWORD;SSL Mode=Require;Trust Server Certificate=true;

# OpenAI API Key (Optional: leave blank to use the built-in deterministic AI engine)
OPENAI_API_KEY=your_openai_api_key_here
OPENAI_MODEL=gpt-4o-mini

# Frontend URL allowed by CORS
FRONTEND_URL=http://localhost:5173

# ASP.NET Core Ports
ASPNETCORE_URLS=http://localhost:5000
```

### 2.2 Frontend Configuration (`frontend/.env`)
```env
# Backend Web API Base URL
VITE_API_BASE_URL=http://localhost:5000/api/v1

# Supabase Credentials
VITE_SUPABASE_URL=https://cskwxvcynuydovzdpros.supabase.co
VITE_SUPABASE_ANON_KEY=sb_publishable_Iuj0mPrfPdUxkqH05IskqA_7-1-ITN2
```

---

## 3. How to Run the Solution

### Starting the Backend (.NET 8 Web API)
```bash
cd backend/src/Equitech.Api
dotnet run
```
- Interactive Swagger UI: `http://localhost:5000/swagger`

### Starting the Frontend (Vue 3 SPA)
```bash
cd frontend
npm run dev
```
- Interactive UI: `http://localhost:5173`
