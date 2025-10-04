--
-- PostgreSQL database dump
--

-- Dumped from database version 17.5
-- Dumped by pg_dump version 17.5

-- Started on 2025-10-01 11:00:24

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
-- TOC entry 218 (class 1259 OID 16578)
-- Name: Categories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Categories" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "DeleteAt" timestamp with time zone,
    "UpdatedAt" timestamp with time zone,
    "IsActive" boolean NOT NULL
);


ALTER TABLE public."Categories" OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 16585)
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
-- TOC entry 217 (class 1259 OID 16573)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 4903 (class 0 OID 16578)
-- Dependencies: 218
-- Data for Name: Categories; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Categories" ("Id", "Name", "CreatedAt", "DeleteAt", "UpdatedAt", "IsActive") FROM stdin;
b1e27c71-2e42-4e1c-8a5f-0f7b69c7e8a1	Electronics	2024-01-15 10:22:00+01	\N	\N	t
4d42b8f2-bcb5-4f6e-9b41-9851b0f76c23	Home Appliances	2024-02-10 12:11:00+01	\N	\N	t
e18f9f91-88cb-4e44-a5b1-49722b3dbf4d	Furniture	2023-11-05 08:40:00+01	\N	\N	t
8c54d471-37c9-4a1a-a2a3-b0ec9e40e651	Tools	2024-03-22 15:30:00+01	\N	\N	t
19a90a61-2684-43de-bd39-21b47f2d51f2	Clothing	2024-05-01 09:00:00+02	\N	\N	t
d9c7a49f-0335-47c6-89a8-77dff03cfb32	Shoes	2024-06-11 17:10:00+02	\N	\N	t
0f7f76b5-12e9-4b31-8029-55d9a69a7df0	Office Supplies	2023-10-02 13:55:00+02	\N	\N	t
39e63f61-2f02-4c60-8aa6-3b623f0f97c2	Toys	2024-07-15 11:20:00+02	\N	\N	t
4aa82e16-8616-4f73-8a45-7f28c271a75e	Cosmetics	2024-08-22 14:45:00+02	\N	\N	t
e812e027-f784-43b2-a61c-f2a8b381c6e4	Cleaning Products	2023-09-19 18:05:00+02	\N	\N	t
ea09fda1-03c2-4c86-94f1-0db2891d407b	Sports Equipment	2024-04-07 10:50:00+02	\N	\N	t
d373dabc-85b9-41de-9023-1832ac8e7b4a	Lighting	2024-01-28 08:10:00+01	\N	\N	t
a9284a8a-05b7-42c8-8bc5-c7d3527a41a2	Car Accessories	2024-05-17 16:22:00+02	\N	\N	t
0b6c2550-233a-4f32-85a3-2135d90a30f6	Groceries	2024-02-12 09:35:00+01	\N	\N	t
cd2b09c6-8f45-4f82-a5f6-d1d931acfc22	Alcohol	2023-12-30 20:55:00+01	\N	\N	f
e7a21f4c-181b-497c-842c-0a7c17b0dc6d	Beverages	2024-06-02 07:25:00+02	\N	\N	t
30b8e5a9-3d6c-42a7-9c8b-5fbe15687ac1	Books	2024-08-05 19:40:00+02	\N	\N	t
abf3e5c1-fb2c-4b3e-b8a2-2de85f1d19b2	Music	2024-09-09 13:00:00+02	\N	\N	f
1a49a3d1-81a8-41e4-a995-9b07bb53d81a	Movies	2024-03-14 21:45:00+01	\N	\N	t
1ff2086f-cf41-4e6c-81cb-f90c879d99a9	Video Games	2024-07-28 15:15:00+02	\N	\N	t
\.


--
-- TOC entry 4904 (class 0 OID 16585)
-- Dependencies: 219
-- Data for Name: Products; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Products" ("Id", "Name", "Description", "StockKeepingUnit", "CategoryId", "Manufacturer", "CreatedAt", "DeletedAt", "UpdatedAt", "IsActive") FROM stdin;
a1c1b2d3-e4f5-4a6b-8c9d-0a1b2c3d4e5f	Smartphone X1	Latest 5G smartphone	ELEC-001	b1e27c71-2e42-4e1c-8a5f-0f7b69c7e8a1	TechCorp	2024-01-15 10:22:00+01	\N	\N	t
b2d2c3e4-f5a6-4b7c-8d9e-1b2c3d4e5f6a	Laptop Pro 15	High-performance laptop	ELEC-002	b1e27c71-2e42-4e1c-8a5f-0f7b69c7e8a1	TechCorp	2024-02-10 12:11:00+01	\N	\N	t
c3e3f4a5-b6c7-4d8e-9f0a-2c3d4e5f6a7b	Microwave Oven	700W microwave	HOME-001	4d42b8f2-bcb5-4f6e-9b41-9851b0f76c23	HomeTech	2024-03-05 09:30:00+01	\N	\N	t
d4f4a5b6-c7d8-4e9f-0a1b-3d4e5f6a7b8c	Refrigerator XL	350L fridge	HOME-002	4d42b8f2-bcb5-4f6e-9b41-9851b0f76c23	HomeTech	2024-03-22 15:30:00+01	\N	\N	t
e5a5b6c7-d8e9-4f0a-1b2c-4e5f6a7b8c9d	Office Chair	Ergonomic chair	FURN-001	e18f9f91-88cb-4e44-a5b1-49722b3dbf4d	FurniCo	2024-05-01 09:00:00+02	\N	\N	t
f6b6c7d8-e9f0-4a1b-2c3d-5f6a7b8c9d0e	Dining Table	Wooden dining table	FURN-002	e18f9f91-88cb-4e44-a5b1-49722b3dbf4d	FurniCo	2024-05-10 11:45:00+02	\N	\N	t
a7c7d8e9-f0a1-4b2c-3d4e-6a7b8c9d0e1f	Hammer	16oz steel hammer	TOOL-001	8c54d471-37c9-4a1a-a2a3-b0ec9e40e651	ToolMaster	2023-11-02 14:15:00+01	\N	\N	t
b8d8e9f0-a1b2-4c3d-4e5f-7b8c9d0e1f2a	Screwdriver Set	10-piece set	TOOL-002	8c54d471-37c9-4a1a-a2a3-b0ec9e40e651	ToolMaster	2024-01-28 08:10:00+01	\N	\N	t
c9e9f0a1-b2c3-4d4e-5f6a-8c9d0e1f2a3b	T-shirt Basic	Cotton T-shirt	CLOTH-001	19a90a61-2684-43de-bd39-21b47f2d51f2	FashionHub	2024-06-11 17:10:00+02	\N	\N	t
d0f0a1b2-c3d4-4e5f-6a7b-9d0e1f2a3b4c	Jeans Classic	Denim jeans	CLOTH-002	19a90a61-2684-43de-bd39-21b47f2d51f2	FashionHub	2024-07-15 11:20:00+02	\N	\N	t
e1a1b2c3-d4e5-4f6a-7b8c-0e1f2a3b4c5d	Running Shoes	Lightweight shoes	SHOE-001	d9c7a49f-0335-47c6-89a8-77dff03cfb32	ShoeBrand	2024-08-22 14:45:00+02	\N	\N	t
f2b2c3d4-e5f6-4a7b-8c9d-1f2a3b4c5d6e	Leather Boots	Winter boots	SHOE-002	d9c7a49f-0335-47c6-89a8-77dff03cfb32	ShoeBrand	2024-09-01 09:50:00+02	\N	\N	t
a3c3d4e5-f6a7-4b8c-9d0e-2a3b4c5d6e7f	Notebook A4	200-page notebook	OFFICE-001	0f7f76b5-12e9-4b31-8029-55d9a69a7df0	OfficeGoods	2024-03-14 21:45:00+01	\N	\N	t
b4d4e5f6-a7b8-4c9d-0e1f-3b4c5d6e7f8a	Pen Set	10-color pens	OFFICE-002	0f7f76b5-12e9-4b31-8029-55d9a69a7df0	OfficeGoods	2024-07-28 15:15:00+02	\N	\N	t
c5e5f6a7-b8c9-4d0e-1f2a-4c5d6e7f8a9b	Building Blocks	Plastic toy set	TOY-001	39e63f61-2f02-4c60-8aa6-3b623f0f97c2	FunToys	2024-01-05 12:00:00+01	\N	\N	t
d6f6a7b8-c9d0-4e1f-2a3b-5d6e7f8a9b0c	Doll	Fashion doll	TOY-002	39e63f61-2f02-4c60-8aa6-3b623f0f97c2	FunToys	2024-02-18 14:30:00+01	\N	\N	t
e7a7b8c9-d0e1-4f2a-3b4c-6e7f8a9b0c1d	Lipstick	Matte lipstick	COS-001	4aa82e16-8616-4f73-8a45-7f28c271a75e	BeautyCo	2024-03-22 15:30:00+01	\N	\N	t
f8b8c9d0-e1f2-4a3b-4c5d-7f8a9b0c1d2e	Foundation	Liquid foundation	COS-002	4aa82e16-8616-4f73-8a45-7f28c271a75e	BeautyCo	2024-04-05 09:20:00+02	\N	\N	t
a9c9d0e1-f2a3-4b4c-5d6e-8a9b0c1d2e3f	Dish Soap	500ml liquid soap	CLEAN-001	e812e027-f784-43b2-a61c-f2a8b381c6e4	CleanHome	2024-05-17 16:22:00+02	\N	\N	t
b0d0e1f2-a3b4-4c5d-6e7f-9b0c1d2e3f4a	Laundry Detergent	2kg powder	CLEAN-002	e812e027-f784-43b2-a61c-f2a8b381c6e4	CleanHome	2024-06-02 07:25:00+02	\N	\N	t
c1e1f2a3-b4c5-4d6e-7f8a-0c1d2e3f4a5b	Soccer Ball	Official size 5	SPORT-001	ea09fda1-03c2-4c86-94f1-0db2891d407b	SportCo	2024-07-05 11:10:00+02	\N	\N	t
d2f2a3b4-c5d6-4e7f-8a9b-1d2e3f4a5b6c	Tennis Racket	Lightweight racket	SPORT-002	ea09fda1-03c2-4c86-94f1-0db2891d407b	SportCo	2024-08-10 13:50:00+02	\N	\N	t
e3a3b4c5-d6e7-4f8a-9b0c-2e3f4a5b6c7d	Desk Lamp	LED lamp	LIGHT-001	d373dabc-85b9-41de-9023-1832ac8e7b4a	LightCo	2024-09-15 10:05:00+02	\N	\N	t
f4b4c5d6-e7f8-4a9b-0c1d-3f4a5b6c7d8e	Ceiling Lamp	Modern ceiling lamp	LIGHT-002	d373dabc-85b9-41de-9023-1832ac8e7b4a	LightCo	2024-10-01 09:15:00+02	\N	\N	t
a5c5d6e7-f8a9-4b0c-1d2e-4a5b6c7d8e9f	Car Cover	Protective car cover	CAR-001	a9284a8a-05b7-42c8-8bc5-c7d3527a41a2	AutoGoods	2024-02-20 12:40:00+01	\N	\N	t
b6d6e7f8-a9b0-4c1d-2e3f-5b6c7d8e9f0a	Car Wax	Liquid wax 500ml	CAR-002	a9284a8a-05b7-42c8-8bc5-c7d3527a41a2	AutoGoods	2024-03-18 14:55:00+01	\N	\N	t
c7e7f8a9-b0c1-4d2e-3f4a-6c7d8e9f0a1b	Organic Rice	1kg pack	GROC-001	0b6c2550-233a-4f32-85a3-2135d90a30f6	FoodCo	2024-04-10 10:30:00+02	\N	\N	t
d8f8a9b0-c1d2-4e3f-4a5b-7d8e9f0a1b2c	Olive Oil	500ml bottle	GROC-002	0b6c2550-233a-4f32-85a3-2135d90a30f6	FoodCo	2024-05-25 16:20:00+02	\N	\N	t
e9a9b0c1-d2e3-4f4a-5b6c-8e9f0a1b2c3d	Red Wine	750ml bottle	ALC-001	cd2b09c6-8f45-4f82-a5f6-d1d931acfc22	WineCo	2024-06-15 19:10:00+02	\N	\N	t
f0b0c1d2-e3f4-4a5b-6c7d-9f0a1b2c3d4e	White Wine	750ml bottle	ALC-002	cd2b09c6-8f45-4f82-a5f6-d1d931acfc22	WineCo	2024-07-30 12:25:00+02	\N	\N	t
\.


--
-- TOC entry 4902 (class 0 OID 16573)
-- Dependencies: 217
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20251001084508_Init	8.0.18
\.


--
-- TOC entry 4752 (class 2606 OID 16584)
-- Name: Categories PK_Categories; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Categories"
    ADD CONSTRAINT "PK_Categories" PRIMARY KEY ("Id");


--
-- TOC entry 4755 (class 2606 OID 16591)
-- Name: Products PK_Products; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Products"
    ADD CONSTRAINT "PK_Products" PRIMARY KEY ("Id");


--
-- TOC entry 4750 (class 2606 OID 16577)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- TOC entry 4753 (class 1259 OID 16597)
-- Name: IX_Products_CategoryId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Products_CategoryId" ON public."Products" USING btree ("CategoryId");


--
-- TOC entry 4756 (class 2606 OID 16592)
-- Name: Products FK_Products_Categories_CategoryId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Products"
    ADD CONSTRAINT "FK_Products_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES public."Categories"("Id") ON DELETE CASCADE;


-- Completed on 2025-10-01 11:00:24

--
-- PostgreSQL database dump complete
--

