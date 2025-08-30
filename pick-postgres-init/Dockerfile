FROM  alpine:latest AS builder 
WORKDIR /app
COPY picking-postgres-init ./
RUN apk add --no-cache openssl

FROM postgres:latest

ENV POSTGRES_USER=postgres
ENV POSTGRES_PASSWORD=admin
ENV POSTGRES_DB=InventoryDb

EXPOSE 5432