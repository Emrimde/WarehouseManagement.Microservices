FROM  alpine:latest AS builder 
WORKDIR /app
COPY product-postgres-init ./
RUN apk add --no-cache openssl

FROM postgres:latest

ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=admin
ENV POSTGRES_DB=InventoryDb

EXPOSE 5432