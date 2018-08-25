-- Table: public."mstUser"

-- DROP TABLE public."mstUser";

CREATE TABLE public."mstUser"
(
    "Id" text COLLATE pg_catalog."default" NOT NULL,
    "Name" text COLLATE pg_catalog."default",
    "Mail" text COLLATE pg_catalog."default",
    "Note" text COLLATE pg_catalog."default",
    "Admin" boolean,
    "InsertDate" date,
    "UpdateDate" date,
    CONSTRAINT "mstUser_pkey" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."mstUser"
    OWNER to miyabis;
COMMENT ON TABLE public."mstUser"
    IS 'ユーザー情報';