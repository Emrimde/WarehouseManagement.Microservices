--
-- PostgreSQL database dump
--

-- Dumped from database version 17.5
-- Dumped by pg_dump version 17.5

-- Started on 2025-08-16 07:23:50

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
-- TOC entry 218 (class 1259 OID 16446)
-- Name: InventoryItems; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."InventoryItems" (
    "Id" uuid NOT NULL,
    "StockKeepingUnit" text NOT NULL,
    "QuantityOnHand" integer NOT NULL,
    "QuantityReserved" integer NOT NULL,
    "UpdatedAt" timestamp with time zone NOT NULL,
    "UnitPrice" numeric
);


ALTER TABLE public."InventoryItems" OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 16441)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 4895 (class 0 OID 16446)
-- Dependencies: 218
-- Data for Name: InventoryItems; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."InventoryItems" ("Id", "StockKeepingUnit", "QuantityOnHand", "QuantityReserved", "UpdatedAt", "UnitPrice") FROM stdin;
35563352-ab62-4ebb-bfb2-cd4edcecd7c4	SONY-WH1000XM5	50	10	2025-08-07 20:02:37.329057+02	3000
\.


--
-- TOC entry 4894 (class 0 OID 16441)
-- Dependencies: 217
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20250805093514_Init	8.0.18
20250816051525_UnitPrice	8.0.18
\.


--
-- TOC entry 4748 (class 2606 OID 16452)
-- Name: InventoryItems PK_InventoryItems; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."InventoryItems"
    ADD CONSTRAINT "PK_InventoryItems" PRIMARY KEY ("Id");


--
-- TOC entry 4746 (class 2606 OID 16445)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


-- Completed on 2025-08-16 07:23:50

--
-- PostgreSQL database dump complete
--

