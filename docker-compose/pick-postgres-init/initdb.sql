--
-- PostgreSQL database dump
--

-- Dumped from database version 17.5
-- Dumped by pg_dump version 17.5

-- Started on 2025-08-21 12:54:32

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
-- TOC entry 219 (class 1259 OID 16525)
-- Name: PickItem; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."PickItem" (
    "Id" uuid NOT NULL,
    "PickTaskId" uuid NOT NULL,
    "SKU" text NOT NULL,
    "Quantity" integer NOT NULL
);


ALTER TABLE public."PickItem" OWNER TO postgres;

--
-- TOC entry 218 (class 1259 OID 16518)
-- Name: PickTasks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."PickTasks" (
    "Id" uuid NOT NULL,
    "OrderId" text NOT NULL,
    "OrderNumber" text,
    "CustomerName" text,
    "Status" integer NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL
);


ALTER TABLE public."PickTasks" OWNER TO postgres;

--
-- TOC entry 217 (class 1259 OID 16513)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 4904 (class 0 OID 16525)
-- Dependencies: 219
-- Data for Name: PickItem; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."PickItem" ("Id", "PickTaskId", "SKU", "Quantity") FROM stdin;
\.


--
-- TOC entry 4903 (class 0 OID 16518)
-- Dependencies: 218
-- Data for Name: PickTasks; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."PickTasks" ("Id", "OrderId", "OrderNumber", "CustomerName", "Status", "CreatedAt") FROM stdin;
\.


--
-- TOC entry 4902 (class 0 OID 16513)
-- Dependencies: 217
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20250821105313_Init	8.0.19
\.


--
-- TOC entry 4755 (class 2606 OID 16531)
-- Name: PickItem PK_PickItem; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."PickItem"
    ADD CONSTRAINT "PK_PickItem" PRIMARY KEY ("Id");


--
-- TOC entry 4752 (class 2606 OID 16524)
-- Name: PickTasks PK_PickTasks; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."PickTasks"
    ADD CONSTRAINT "PK_PickTasks" PRIMARY KEY ("Id");


--
-- TOC entry 4750 (class 2606 OID 16517)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 4753 (class 1259 OID 16537)
-- Name: IX_PickItem_PickTaskId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_PickItem_PickTaskId" ON public."PickItem" USING btree ("PickTaskId");


--
-- TOC entry 4756 (class 2606 OID 16532)
-- Name: PickItem FK_PickItem_PickTasks_PickTaskId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."PickItem"
    ADD CONSTRAINT "FK_PickItem_PickTasks_PickTaskId" FOREIGN KEY ("PickTaskId") REFERENCES public."PickTasks"("Id") ON DELETE CASCADE;


-- Completed on 2025-08-21 12:54:32

--
-- PostgreSQL database dump complete
--

