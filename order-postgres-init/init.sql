--
-- PostgreSQL database dump
--

-- Dumped from database version 17.5
-- Dumped by pg_dump version 17.5

-- Started on 2025-08-15 21:22:18

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 219 (class 1259 OID 16466)
-- Name: OrderItems; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."OrderItems" (
    "Id" uuid NOT NULL,
    "OrderId" uuid NOT NULL,
    "SKU" text NOT NULL,
    "ProductName" text NOT NULL,
    "UnitPrice" numeric NOT NULL,
    "Quantity" integer NOT NULL
);


ALTER TABLE public."OrderItems" OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 16459)
-- Name: Orders; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Orders" (
    "Id" uuid NOT NULL,
    "OrderNumber" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone,
    "Status" integer NOT NULL,
    "CustomerName" text NOT NULL,
    "CustomerEmail" text NOT NULL
);


ALTER TABLE public."Orders" OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 16454)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 4904 (class 0 OID 16466)
-- Dependencies: 219
-- Data for Name: OrderItems; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."OrderItems" ("Id", "OrderId", "SKU", "ProductName", "UnitPrice", "Quantity") FROM stdin;
\.


--
-- TOC entry 4903 (class 0 OID 16459)
-- Dependencies: 218
-- Data for Name: Orders; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Orders" ("Id", "OrderNumber", "CreatedAt", "UpdatedAt", "Status", "CustomerName", "CustomerEmail") FROM stdin;
\.


--
-- TOC entry 4902 (class 0 OID 16454)
-- Dependencies: 217
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20250815154402_Init	8.0.19
\.


--
-- TOC entry 4755 (class 2606 OID 16472)
-- Name: OrderItems PK_OrderItems; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OrderItems"
    ADD CONSTRAINT "PK_OrderItems" PRIMARY KEY ("Id");


--
-- TOC entry 4752 (class 2606 OID 16465)
-- Name: Orders PK_Orders; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Orders"
    ADD CONSTRAINT "PK_Orders" PRIMARY KEY ("Id");


--
-- TOC entry 4750 (class 2606 OID 16458)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 4753 (class 1259 OID 16478)
-- Name: IX_OrderItems_OrderId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_OrderItems_OrderId" ON public."OrderItems" USING btree ("OrderId");


--
-- TOC entry 4756 (class 2606 OID 16473)
-- Name: OrderItems FK_OrderItems_Orders_OrderId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."OrderItems"
    ADD CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES public."Orders"("Id") ON DELETE CASCADE;


-- Completed on 2025-08-15 21:22:19

--
-- PostgreSQL database dump complete
--

