-- FUNCTION: public."funcTest"(integer, integer)

 DROP FUNCTION public."funcTest"(integer, integer);

CREATE OR REPLACE FUNCTION public."funcTest"(
	param1 integer,
	param2 integer)
    RETURNS TABLE("Id" text, "Name" text) 
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    ROWS 1000
AS $BODY$

BEGIN
	RETURN QUERY
	SELECT
		CAST(param1 AS text) AS "Id"
        , CAST(param2 AS text) AS "Name";
END

$BODY$;

ALTER FUNCTION public."funcTest"(integer, integer)
    OWNER TO miyabis;
