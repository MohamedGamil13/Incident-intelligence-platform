#  Incident Intelligence Platform

A progressive **Incident Management and Intelligence Platform** built with **ASP.NET Core, Angular, SQL Server, Redis, RabbitMQ, Kafka, Spark, Airflow, AI/RAG, Docker, and Azure**.

The project is designed as a long-term engineering journey, evolving from a structured CRUD-based incident management API into a **production-oriented, event-driven, data-engineered, AI-powered platform**.

The development is organized into **27 progressive phases grouped into 9 versions**, where each version represents a demonstrable product state.

> **Current Status:** Phase 5 — Logs & Correlation
> **Completed:** Phases 1–4
> **Next:** Phase 6 — Background Processing

---

##  Project Overview

The platform is designed to help engineering teams:

* Manage and track incidents
* Monitor services and operational logs
* Detect incidents automatically
* Correlate logs with deployments and incidents
* Process incidents asynchronously
* Analyze incidents using AI
* Retrieve similar historical incidents using RAG
* Build operational analytics and SRE metrics
* Process large-scale event streams
* Maintain a data lake and data warehouse
* Monitor system health and observability
* Eventually deploy the complete platform to the cloud

The project follows a **build-as-you-go philosophy**:

> Build only what the current phase requires. A phase is complete when its backend and frontend outputs work end-to-end.

The complete roadmap is documented in the project proposal located under `/Docs`.

---

#  Repository Structure

```text
Incident-Intelligence-Platform/
│
├── Backend/
│   │
│   ├── Core/
│   │   ├── Domain/
│   │   ├── ServiceAbstraction/
│   │   └── Service/
│   │
│   ├── Infrastructure/
│   │   ├── Persistence/
│   │   └── Presentation/
│   │
│   └── API/
│
├── Frontend/
│   └── Angular/
│
├── Docs/
│   ├── Proposal/
│   ├── TestData/
│   └── ...
│
└── README.md
```

---

#  Backend

The backend is built using **ASP.NET Core Web API** following an **Onion Architecture** approach.

```text
Backend
│
├── Core
│   ├── Domain
│   ├── ServiceAbstraction
│   └── Service
│
├── Infrastructure
│   ├── Persistence
│   └── Presentation
│
└── API
```

## Core

The Core layer contains the application's business logic and contracts and is designed to remain independent from infrastructure concerns.

### Domain

Contains the core domain model:

* Entities
* Domain concepts
* Business rules
* Domain-related abstractions

Examples include:

```text
Incident
Service
IncidentEvent
Log
Deployment
```

### ServiceAbstraction

Contains service contracts and abstractions used by the application.

This layer helps keep the application logic independent from concrete infrastructure implementations.

### Service

Contains the implementation of application/business services.

Responsibilities include:

* Business operations
* Application workflows
* DTO handling
* Validation-related application logic
* Coordination between domain and infrastructure abstractions

---

#  Infrastructure

The Infrastructure layer contains implementations that depend on external technologies.

## Persistence

Responsible for data access and persistence.

Current/future technologies include:

* Entity Framework Core
* SQL Server
* Database migrations
* Repository/data-access implementations
* Query optimization
* Redis caching

The project will progressively introduce more advanced database engineering, including:

* Indexing
* Query execution plans
* Concurrency control
* Transactions
* Keyset pagination
* Performance benchmarking

## Presentation

Contains presentation-related infrastructure such as:

* Controllers
* API response handling
* Middleware
* Validation integration
* Authentication/authorization presentation concerns
* API-specific configuration

The API layer remains responsible for composing the application rather than containing the core business rules.

---

#  API

The API project is the entry point of the backend application.

It is responsible for:

* Application startup
* Dependency Injection configuration
* Middleware pipeline
* Authentication & Authorization configuration
* Endpoint exposure
* API documentation
* Application configuration

The API exposes the functionality implemented through the Core and Infrastructure layers.

---

#  Frontend

The frontend is built using:

* **Angular**
* **TypeScript**
* Angular Material
* RxJS
* Angular Signals
* Chart.js / ngx-charts
* SignalR
* Playwright

The Angular project has currently been scaffolded and is intentionally kept minimal while backend development progresses.

The frontend will gradually evolve alongside the backend phases.

Planned features include:

* Authentication
* Incident management
* Incident timeline
* Logs explorer
* Service health
* Dashboard
* Real-time incident updates
* AI Root Cause Analysis
* Analytics
* Data Quality monitoring
* Pipeline monitoring
* Administration

---

#  Documentation

The `Docs` directory contains project documentation and supporting data.

```text
Docs/
│
├── Proposal/
│   ├── Incident_Intelligence_Platform_Proposal_Updated.pdf
│   └── Previous Proposal
│
└── TestData/
```

The updated proposal defines the project's **27 phases across 9 versions**, including both backend and frontend deliverables.

Test data used during development and testing is also maintained inside the documentation area.

---

#  Development Roadmap

The project is divided into **9 versions and 27 phases**.

| Version | Focus                                 | Phases |
| ------- | ------------------------------------- | -----: |
| **MVP** | Incident Tracker                      |    1–4 |
| **V2**  | Detection & Performance               |    5–7 |
| **V3**  | Event-Driven, AI & Observability      |   8–10 |
| **V4**  | Resilience & Database Engineering     |  11–12 |
| **V5**  | Streaming & Data Lake                 |  13–15 |
| **V6**  | Processing, Warehouse & Orchestration |  16–18 |
| **V7**  | Data Quality, Analytics & Advanced AI |  19–21 |
| **V8**  | Security, Testing & Load Testing      |  22–24 |
| **V9**  | Production Release                    |  25–27 |

The proposal defines the nine release states from the initial Incident Tracker MVP through the final production architecture.

---

#  Current Progress

### MVP — Incident Tracker

* [x] Phase 1 — Core Incident API
* [x] Phase 2 — Professional API
* [x] Phase 3 — Authentication & Authorization
* [x] Phase 4 — Incident Timeline & Domain Logic

### V2 — Detection & Performance

* [ ] Phase 5 — Logs & Correlation **(In Progress)**
* [ ] Phase 6 — Background Processing
* [ ] Phase 7 — Redis & Performance

### V3 — Event-Driven, AI & Observable

* [ ] Phase 8 — Event-Driven Architecture
* [ ] Phase 9 — AI Root Cause Analysis
* [ ] Phase 10 — Observability & Production Readiness

### V4 — Resilient & Database-Hardened

* [ ] Phase 11 — Advanced Reliability
* [ ] Phase 12 — Advanced Database Engineering

### V5 — Streaming & Data Lake

* [ ] Phase 13 — Kafka Event Streaming
* [ ] Phase 14 — Event Streaming Pipeline
* [ ] Phase 15 — Data Lake / Data Storage Layer

### V6 — Processing, Warehouse & Orchestration

* [ ] Phase 16 — Spark Data Processing
* [ ] Phase 17 — Data Warehouse
* [ ] Phase 18 — Airflow / Data Orchestration

### V7 — Trusted Data, Analytics & Advanced AI

* [ ] Phase 19 — Data Quality
* [ ] Phase 20 — Analytics Layer
* [ ] Phase 21 — Advanced AI / RAG

### V8 — Secure, Tested & Proven

* [ ] Phase 22 — Security Hardening
* [ ] Phase 23 — Testing Strategy
* [ ] Phase 24 — Performance & Load Testing

### V9 — Production Release

* [ ] Phase 25 — Docker & Deployment
* [ ] Phase 26 — Cloud
* [ ] Phase 27 — Production Architecture

The current proposal specifically identifies Phases 1–4 as completed and Phase 5 as in progress.

---

# 🛠️ Technology Stack

## Backend

* C#
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* FluentValidation
* Serilog
* JWT / ASP.NET Core Identity

## Caching & Messaging

* Redis
* Hangfire
* RabbitMQ
* SignalR
* Kafka
* Outbox Pattern

## Data Engineering

* Parquet
* MinIO
* Apache Spark
* PySpark
* Apache Airflow
* Data Warehouse
* Great Expectations / Soda

## AI

* LLM APIs
* RAG
* Embeddings
* Vector Search
* pgvector / Qdrant / Azure AI Search

## Frontend

* Angular
* TypeScript
* Angular Material
* RxJS
* Angular Signals
* Chart.js / ngx-charts
* SignalR

## Testing & Quality

* xUnit / backend testing
* Testcontainers
* Playwright
* k6
* API Contract Testing

## DevOps & Cloud

* Docker
* Docker Compose
* GitHub Actions / Azure DevOps
* Azure
* Azure SQL
* Azure Cache for Redis
* Azure Key Vault
* OpenTelemetry
* Jaeger / Grafana

The complete technology stack is introduced progressively throughout the roadmap rather than being implemented all at once.

---

#  Target Architecture

The final architecture is intended to evolve toward:

```text
                    ┌──────────────┐
                    │   Angular    │
                    │   Frontend   │
                    └──────┬───────┘
                           │
                           ▼
                    ┌──────────────┐
                    │ API Gateway  │
                    └──────┬───────┘
                           │
                           ▼
                ┌──────────────────────┐
                │    Incident API      │
                │       + Auth         │
                └──────────┬───────────┘
                           │
             ┌─────────────┼─────────────┐
             ▼             ▼             ▼
        ┌─────────┐   ┌─────────┐   ┌──────────┐
        │ SQL     │   │  Redis  │   │ RabbitMQ │
        │ Server  │   │         │   │          │
        └─────────┘   └─────────┘   └────┬─────┘
                                         │
                                         ▼
                                  ┌─────────────┐
                                  │   Workers   │
                                  └──────┬──────┘
                                         │
                                         ▼
                                     ┌───────┐
                                     │ Kafka │
                                     └───┬───┘
                                         │
                         ┌───────────────┼───────────────┐
                         ▼               ▼               ▼
                     Consumers         Spark        Data Pipeline
                         │               │               │
                         └───────────────┼───────────────┘
                                         ▼
                              ┌─────────────────────┐
                              │ Data Lake /         │
                              │ Data Warehouse      │
                              └──────────┬──────────┘
                                         │
                                         ▼
                                  ┌─────────────┐
                                  │  Analytics  │
                                  └─────────────┘

        Incidents + Logs + History
                    │
                    ▼
              ┌──────────┐
              │   RAG    │
              └────┬─────┘
                   ▼
                 ┌─────┐
                 │ LLM │
                 └──┬──┘
                    ▼
             Root Cause Analysis
```

This represents the target architecture described for Phase 27, where operational data, event streaming, data processing, analytics, RAG, AI analysis, and observability are brought together into one production architecture.

---

# 🎯 Engineering Goals

This project is not intended to be just another CRUD application.

The main goal is to progressively explore and implement real backend and data-engineering concepts:

* Clean architecture
* Domain-driven design concepts
* REST API design
* Authentication & authorization
* Validation
* Error handling
* Database design
* Query optimization
* Caching
* Background processing
* Message queues
* Event-driven architecture
* Event streaming
* Distributed processing
* Data lakes
* Data warehouses
* Data quality
* Observability
* Reliability engineering
* AI / RAG
* Automated testing
* Load testing
* Containerization
* CI/CD
* Cloud deployment

---

# 📈 Development Philosophy

The project follows a **progressive architecture approach**.

Instead of introducing every technology from the beginning, each technology is introduced when the current problem requires it.

For example:

```text
CRUD
  ↓
Professional API
  ↓
Authentication
  ↓
Domain Rules
  ↓
Operational Logs
  ↓
Background Processing
  ↓
Redis
  ↓
RabbitMQ
  ↓
AI
  ↓
Observability
  ↓
Kafka
  ↓
Data Lake
  ↓
Spark
  ↓
Data Warehouse
  ↓
Airflow
  ↓
Data Quality
  ↓
Advanced RAG
  ↓
Testing & Load Testing
  ↓
Docker
  ↓
Cloud
  ↓
Production Architecture
```

Each phase builds on the previous one rather than rewriting the system from scratch.

---

#  Getting Started

> Setup instructions will evolve as more infrastructure is introduced.

### Prerequisites

Current/future development requires some or all of:

* .NET SDK
* SQL Server
* Node.js
* Angular CLI
* Redis
* Docker
* Docker Compose

Additional infrastructure such as RabbitMQ, Kafka, Spark, Airflow, and MinIO will be introduced in later phases.

### Clone the Repository

```bash
git clone <repository-url>
cd Incident-Intelligence-Platform
```

### Backend

Navigate to the API project:

```bash
cd Backend/API
```

Then restore dependencies:

```bash
dotnet restore
```

Run the API:

```bash
dotnet run
```

Swagger will be available when the API starts in the configured development environment.

### Frontend

The Angular application will be developed under:

```text
Frontend/
```

---

#  Documentation

The main project proposal contains:

* Project scope
* 27 development phases
* 9 release versions
* Backend outputs
* Frontend outputs
* Technology roadmap
* Demo scenarios
* Final architecture

The proposal also defines the immediate milestone as completing **Phase 5 — Logs & Correlation**, followed by Phase 6 to complete V2.

---

#  Project Status

This project is actively being developed as a long-term engineering and portfolio project.

The repository will evolve continuously as each phase is implemented.

**Current milestone:**

```text
MVP ────────────────────────► V2
 ✓ Phase 1
 ✓ Phase 2
 ✓ Phase 3
 ✓ Phase 4
 → Phase 5
   Phase 6
   Phase 7
```

---

#  License

License information will be added as the project progresses.
