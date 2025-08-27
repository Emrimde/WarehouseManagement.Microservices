# Warehouse Management Microservices

This repository contains a set of microservices for managing a warehouse, including services for orders, products, inventory, and picking tasks. Each service runs in a separate Docker container and uses PostgreSQL for persistence, Redis for caching, and RabbitMQ for messaging.

## Services

- **PickingMicroservice.API** – Manages picking tasks for orders. Connects to `PickingDb`.
- **OrderMicroservice.API** – Handles orders. Connects to `OrdersDb`.
- **ProductMicroservice.API** – Manages products. Connects to `ProductsDb` and Redis.
- **InventoryMicroservice.API** – Handles inventory. Connects to `Inventoriesdb` and Redis.
- **ApiGateway** – Gateway for routing requests to microservices.
- **RabbitMQ** – Messaging service for event-driven communication.
- **Redis** – Caching service for GET methods.

## Databases

Each service has its own PostgreSQL container:

- `picking-db` → `PickingDb`  
- `order-db` → `OrdersDb`  
- `product-db` → `ProductsDb`  
- `inventory-db` → `Inventoriesdb`  

## Docker Compose Setup

The main entry point for running the entire application is the docker-compose folder at the root of the repository.

1.Open a terminal and navigate to the root of the repository.
2.Navigate to the docker-compose folder:
3.Start all microservices using Docker Compose by command: docker-compose up 
4.Wait approximately 20-30 seconds ( It's because of rabbit mq message broker. Microservices are trying to connect with RabbitMQ server)

## Testing the Application
Once all containers are running, you can access the APIs through the API Gateway or directly via the exposed ports which are defined in docker-compose.yml.

API Gateway base url:
http://localhost:5000

base url for each microservice(http):
1.http://localhost:4001 - order microservice
2.http://localhost:6001 - product microservice  
3.http://localhost:7001 - inventory microservice
4.http://localhost:8000 - picking microservice
