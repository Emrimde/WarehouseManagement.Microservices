--
-- PostgreSQL database dump
--

-- Dumped from database version 17.5
-- Dumped by pg_dump version 17.5

-- Started on 2025-10-02 20:17:12

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
    "UnitPrice" numeric,
    "ProductName" text DEFAULT ''::text NOT NULL
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
-- TOC entry 4896 (class 0 OID 16446)
-- Dependencies: 218
-- Data for Name: InventoryItems; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."InventoryItems" ("Id", "StockKeepingUnit", "QuantityOnHand", "QuantityReserved", "UpdatedAt", "UnitPrice", "ProductName") FROM stdin;
35563352-ab62-4ebb-bfb2-cd4edcecd7c4	SONY-WH1000XM5	50	10	2025-08-07 20:02:37.329057+02	3000	Sony
f47ac10b-58cc-4372-a567-0e02b2c3d479	ELEC-001	80	5	2025-10-02 20:14:41.836517+02	699.99	Smartphone X1
b3b2f8a1-6f2c-4a4d-9a8b-1c2d3e4f5a60	ELEC-002	20	2	2025-10-02 20:14:41.836517+02	1299.99	Laptop Pro 15
d2e4c3b4-7a6f-4c5b-8e1a-9f0b1c2d3e4f	HOME-001	35	1	2025-10-02 20:14:41.836517+02	149.99	Microwave Oven
a1b2c3d4-5e6f-7a8b-9c0d-1e2f3a4b5c6d	HOME-002	15	0	2025-10-02 20:14:41.836517+02	899.99	Refrigerator XL
c1d2e3f4-5678-49ab-cdef-0123456789ab	FURN-001	60	4	2025-10-02 20:14:41.836517+02	129.99	Office Chair
e1f2a3b4-6789-4cde-ab01-23456789abcd	FURN-002	10	0	2025-10-02 20:14:41.836517+02	499.99	Dining Table
f1e2d3c4-89ab-4def-0123-456789abcdef	TOOL-001	150	10	2025-10-02 20:14:41.836517+02	19.99	Hammer
01234567-89ab-4cde-0123-456789abcdef	TOOL-002	120	8	2025-10-02 20:14:41.836517+02	24.99	Screwdriver Set
123e4567-e89b-12d3-a456-426614174000	CLOTH-001	200	25	2025-10-02 20:14:41.836517+02	9.99	T-shirt Basic
223e4567-e89b-12d3-a456-426614174001	CLOTH-002	140	10	2025-10-02 20:14:41.836517+02	49.99	Jeans Classic
323e4567-e89b-12d3-a456-426614174002	SHOE-001	90	6	2025-10-02 20:14:41.836517+02	89.99	Running Shoes
423e4567-e89b-12d3-a456-426614174003	SHOE-002	40	3	2025-10-02 20:14:41.836517+02	159.99	Leather Boots
523e4567-e89b-12d3-a456-426614174004	OFFICE-001	500	40	2025-10-02 20:14:41.836517+02	3.49	Notebook A4
623e4567-e89b-12d3-a456-426614174005	OFFICE-002	300	20	2025-10-02 20:14:41.836517+02	5.99	Pen Set
723e4567-e89b-12d3-a456-426614174006	TOY-001	80	5	2025-10-02 20:14:41.836517+02	29.99	Building Blocks
823e4567-e89b-12d3-a456-426614174007	TOY-002	70	6	2025-10-02 20:14:41.836517+02	24.99	Doll
923e4567-e89b-12d3-a456-426614174008	COS-001	180	12	2025-10-02 20:14:41.836517+02	14.99	Lipstick
a23e4567-e89b-12d3-a456-426614174009	COS-002	160	9	2025-10-02 20:14:41.836517+02	19.99	Foundation
b23e4567-e89b-12d3-a456-42661417400a	CLEAN-001	220	18	2025-10-02 20:14:41.836517+02	3.99	Dish Soap
c23e4567-e89b-12d3-a456-42661417400b	CLEAN-002	140	12	2025-10-02 20:14:41.836517+02	12.99	Laundry Detergent
d23e4567-e89b-12d3-a456-42661417400c	SPORT-001	110	6	2025-10-02 20:14:41.836517+02	19.99	Soccer Ball
e23e4567-e89b-12d3-a456-42661417400d	SPORT-002	45	2	2025-10-02 20:14:41.836517+02	79.99	Tennis Racket
f23e4567-e89b-12d3-a456-42661417400e	LIGHT-001	75	5	2025-10-02 20:14:41.836517+02	34.99	Desk Lamp
023e4567-e89b-12d3-a456-42661417400f	LIGHT-002	40	1	2025-10-02 20:14:41.836517+02	59.99	Ceiling Lamp
133e4567-e89b-12d3-a456-426614174010	CAR-001	60	3	2025-10-02 20:14:41.836517+02	39.99	Car Cover
233e4567-e89b-12d3-a456-426614174011	CAR-002	130	10	2025-10-02 20:14:41.836517+02	12.99	Car Wax
333e4567-e89b-12d3-a456-426614174012	GROC-001	250	20	2025-10-02 20:14:41.836517+02	8.99	Organic Rice
433e4567-e89b-12d3-a456-426614174013	GROC-002	180	15	2025-10-02 20:14:41.836517+02	12.99	Olive Oil
533e4567-e89b-12d3-a456-426614174014	ALC-001	95	7	2025-10-02 20:14:41.836517+02	15.99	Red Wine
633e4567-e89b-12d3-a456-426614174015	ALC-002	110	8	2025-10-02 20:14:41.836517+02	13.99	White Wine
\.


--
-- TOC entry 4895 (class 0 OID 16441)
-- Dependencies: 217
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20250805093514_Init	8.0.18
20250816051525_UnitPrice	8.0.18
20250906083057_productName	8.0.18
\.


--
-- TOC entry 4749 (class 2606 OID 16452)
-- Name: InventoryItems PK_InventoryItems; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."InventoryItems"
    ADD CONSTRAINT "PK_InventoryItems" PRIMARY KEY ("Id");


--
-- TOC entry 4747 (class 2606 OID 16445)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


-- Completed on 2025-10-02 20:17:13

--
-- PostgreSQL database dump complete
--

