# MetersApp

## Quick Start

1. Access the deployed application:
- **Frontend**: http://89.22.226.142:3000
- **GraphQL Playground**: http://89.22.226.142:8084/graphql
- **Prometheus** (if monitoring enabled): http://89.22.226.142:9090/targets
- **Grafana** (if monitoring enabled): http://89.22.226.142:5000 (admin/admin)

### Running the Application

### Prerequisites
- Docker and Docker Compose installed
- Git

1. Clone the repository:
```bash
git clone <repository-url>
cd MetersApp
```

2. Start all core services:
```bash
docker compose up -d
```

3. (Optional) Start with Prometheus and Grafana monitoring:
```bash
docker compose --profile monitoring up -d
```

4. Access the application:
- **Frontend**: http://localhost:3000
- **GraphQL Playground**: http://localhost:8084/graphql
- **RabbitMQ Management**: http://localhost:15672 (guest/guest)
- **Prometheus** (if monitoring enabled): http://localhost:9090/targets
- **Grafana** (if monitoring enabled): http://localhost:5000 (admin/admin)

### Stopping the Application
```bash
docker compose down
```

To clear everything:
```bash
docker compose down --rmi all -v
```

## Backend Services

| Service | Purpose |
|---------|---------|
| **DataIngestor** | Polls external WeakApp API and publishes sensor data to RabbitMQ |
| **DataProcessor** | Consumes RabbitMQ messages, processes and stores data in PostgreSQL |
| **GraphQLGateway** | Provides GraphQL API for querying historical sensor data |
| **Notifications** | Real-time notifications via SignalR WebSockets |
| **WeakAppApi** | External API providing sensor data (pre-built image) |

## Technology Stack

### Backend (.NET 10)
- **.NET 10.0** - Primary framework
- **MassTransit + RabbitMQ** - Message bus for async communication
- **HotChocolate** - GraphQL server with pagination, filtering, sorting
- **Entity Framework Core** - ORM with PostgreSQL provider
- **SignalR** - Real-time WebSocket communication
- **Polly** - Resilience and retry policies
- **Serilog** - Structured logging

### Frontend (React + TypeScript)
- **React 19** - UI framework
- **TypeScript 5.9** - Type safety
- **Vite 7** - Build tool and dev server
- **Apollo Client 4** - GraphQL data fetching
- **SignalR** - Real-time WebSocket connections
- **Tailwind CSS** - Styling
- **Recharts** - Data visualization
- **wouter** - Lightweight routing

### Infrastructure
- **PostgreSQL 18** - Primary database
- **RabbitMQ** - Message broker (MassTransit image)

## Development

### Backend Development

Navigate to the Backend directory:
```bash
cd Backend/Services
```

Build the solution:
```bash
dotnet build MetersApp.sln
```

Run tests:
```bash
dotnet test
```

### Frontend Development

Navigate to the Frontend directory:
```bash
cd Frontend
```

Install dependencies (requires pnpm):
```bash
pnpm install
```

Start development server:
```bash
pnpm dev
```

Run linting:
```bash
pnpm lint:check
```

Fix linting issues:
```bash
pnpm lint:fix
```

Type checking:
```bash
pnpm typecheck
```

Generate GraphQL types:
```bash
pnpm codegen
```

## Data Flow

1. **Data Ingestion**: DataIngestor service polls WeakApp API every 10 seconds (configurable)
2. **Message Publishing**: Sensor data is published to RabbitMQ as `ProcessSensorDataBatch` messages
3. **Data Processing**: DataProcessor consumes messages and stores data in PostgreSQL
4. **Event Broadcasting**: After storage, `NewSensorDataEvent` is published to RabbitMQ
5. **Real-time Updates**: Notifications service broadcasts events to connected frontend clients via SignalR
6. **Historical Queries**: Frontend queries historical data via GraphQL API

## Project Structure

```
MetersApp/
├── Backend/
│   └── Services/
│       ├── DataIngestorService/      # API polling & message publishing
│       ├── DataProcessorService/     # Message consumption & data storage
│       ├── GraphQLGatewayService/    # GraphQL API gateway
│       ├── NotificationsService/     # SignalR real-time notifications
│       ├── Shared/                   # Common libraries (enums, messages)
│       └── Tests/                    # Unit and integration tests
├── Frontend/                         # React + TypeScript SPA
│   ├── src/
│   │   ├── app/                      # App entry, router, providers
│   │   ├── components/               # Reusable UI components
│   │   ├── widgets/                  # Feature widgets
│   │   ├── pages/                    # Page components
│   │   └── shared/                   # Utils, hooks, GraphQL client
│   └── public/                       # Static assets
├── docker-compose.yml                # Local service orchestration
├── docker-compose.deploy.yml         # Production deployment orchestration
├── prometheus.yml                    # Prometheus scrape configuration
├── .env                              # Environment configuration
└── .github/workflows/                # CI/CD pipelines
```

## CI/CD

The project includes GitHub Actions workflows:

- **Backend Checks** - Build, test, and analyze backend code
- **Frontend Checks** - Lint, type check, and build frontend
- **Deploy** - Build, push, and deploy Docker images to the server
- **Sonar** - Code quality analysis

### Deployment Configuration

The deploy workflow supports the following GitHub repository variables:

| Variable | Description | Default |
|----------|-------------|---------|
| `DEPLOY_MONITORING` | Set to `true` to include Prometheus and Grafana in deployment | Disabled |
| `VITE_API_BASE_URL` | Frontend GraphQL API URL | - |
| `VITE_SENSORS_HUB_URL` | Frontend SignalR hub URL | - |
| `WEAKAPP_REQUEST_INTERVAL` | Polling interval for WeakApp API | - |
| `MIGRATE_DB_ON_STARTUP` | Run database migrations on startup | - |
| `MIGRATION_MAX_RETRIES` | Maximum migration retry attempts | - |
| `CORS_ALLOWED_ORIGINS` | Allowed CORS origins | - |

## API Documentation

### GraphQL Queries

Example query for air quality data:
```graphql
query GetAirQuality($locationId: LocationType!) {
  airQuality(locationId: $locationId) {
    items {
      id
      timestamp
      co2
      pm25
      humidity
    }
  }
}
```

### SignalR Hub

Connect to the sensors hub at: `/hubs/sensors`

Messages are broadcast to all connected clients when new sensor data is available.
