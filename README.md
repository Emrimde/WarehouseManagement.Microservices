# Warehouse Management Microservices

This repository contains a set of microservices for managing a warehouse, including services for orders, products, inventory, and picking tasks. Each service runs in a separate Docker container and uses PostgreSQL for persistence, Redis for caching, and RabbitMQ for messaging.

## Services

- **PickingMicroservice.API** – Manages picking tasks for orders. Connects to `PickingDb`.
- **OrderMicroservice.API** – Handles orders. Connects to `OrdersDb`.
- **ProductMicroservice.API** – Manages products. Connects to `ProductsDb` and Redis.
- **InventoryMicroservice.API** – Handles inventory. Connects to `Inventoriesdb` and Redis.
- **ApiGateway** – Gateway for routing requests to microservices.
- **RabbitMQ** – Messaging service for event-driven communication.
- **Redis** – Caching service for products and inventory.

## Database Initialization

Each service has its own PostgreSQL container:

- `picking-db` → `PickingDb`  
- `order-db` → `OrdersDb`  
- `product-db` → `ProductsDb`  
- `inventory-db` → `Inventoriesdb`  

## Docker Compose Setup

Run all microservices and databases using Docker Compose in picking microservice:
