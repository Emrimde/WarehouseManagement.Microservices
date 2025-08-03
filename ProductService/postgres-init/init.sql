--
-- PostgreSQL database dump
--

-- Dumped from database version 17.5
-- Dumped by pg_dump version 17.5

-- Started on 2025-07-30 09:27:02

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
-- TOC entry 218 (class 1259 OID 16420)
-- Name: Categories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Categories" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL
);


ALTER TABLE public."Categories" OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16427)
-- Name: Products; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Products" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Description" text NOT NULL,
    "StockKeepingUnit" text NOT NULL,
    "CategoryId" uuid NOT NULL,
    "Manufacturer" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "DeletedAt" timestamp with time zone,
    "UpdatedAt" timestamp with time zone,
    "IsActive" boolean NOT NULL
);


ALTER TABLE public."Products" OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 16415)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 4903 (class 0 OID 16420)
-- Dependencies: 218
-- Data for Name: Categories; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Categories" ("Id", "Name") FROM stdin;
d868a700-cb95-4271-b6e8-75dfff29991f	Laptops
2ec9fc36-8f54-4692-a9f3-4322e1704453	Headphones
b7c713a5-cb9c-48ef-8c25-8cfa417d3f6f	Monitors
\.


--
-- TOC entry 4904 (class 0 OID 16427)
-- Dependencies: 219
-- Data for Name: Products; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Products" ("Id", "Name", "Description", "StockKeepingUnit", "CategoryId", "Manufacturer", "CreatedAt", "DeletedAt", "UpdatedAt", "IsActive") FROM stdin;
610f0714-6718-4729-89e9-a614f1addd53	Apple MacBook Pro 14"	Professional laptop with Apple M3 chip, 16GB RAM, 512GB SSD.	AP-MBP14-2025	d868a700-cb95-4271-b6e8-75dfff29991f	Apple	2025-07-29 10:00:00+02	\N	\N	t
3e354d59-ef38-467a-a729-1d926bac40eb	Sony WH-1000XM5	Wireless noise-cancelling headphones with 30 hours battery life.	SONY-WH1000XM5	2ec9fc36-8f54-4692-a9f3-4322e1704453	Sony	2025-07-29 10:10:00+02	\N	\N	t
08712862-ee63-4d55-b1f9-99cf953424bb	Dell UltraSharp U2723QE	27-inch 4K monitor with IPS Black panel, USB-C hub.	DELL-U2723QE	b7c713a5-cb9c-48ef-8c25-8cfa417d3f6f	Dell	2025-07-29 10:20:00+02	\N	\N	t
0a6b6f02-1a20-4d05-b87d-f09c1549e6bc	string	string	string	d868a700-cb95-4271-b6e8-75dfff29991f	string	2025-07-29 15:28:23.232215+02	\N	\N	f
\.


--
-- TOC entry 4902 (class 0 OID 16415)
-- Dependencies: 217
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20250728180214_Init	8.0.18
\.


--
-- TOC entry 4752 (class 2606 OID 16426)
-- Name: Categories PK_Categories; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Categories"
    ADD CONSTRAINT "PK_Categories" PRIMARY KEY ("Id");


--
-- TOC entry 4755 (class 2606 OID 16433)
-- Name: Products PK_Products; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Products"
    ADD CONSTRAINT "PK_Products" PRIMARY KEY ("Id");


--
-- TOC entry 4750 (class 2606 OID 16419)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 4753 (class 1259 OID 16439)
-- Name: IX_Products_CategoryId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Products_CategoryId" ON public."Products" USING btree ("CategoryId");


--
-- TOC entry 4756 (class 2606 OID 16434)
-- Name: Products FK_Products_Categories_CategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Products"
    ADD CONSTRAINT "FK_Products_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES public."Categories"("Id") ON DELETE CASCADE;


-- Completed on 2025-07-30 09:27:02

--
-- PostgreSQL database dump complete
--

