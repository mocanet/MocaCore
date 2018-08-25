USE `MocaTest`;
DROP procedure IF EXISTS `DaoUser_Find`;

DELIMITER $$
USE `MocaTest`$$
CREATE DEFINER=`miyabis`@`%` PROCEDURE `DaoUser_Find`(
	IN param1 int,
	IN param2 int
)
BEGIN

	SELECT
		CAST(param1 AS char(50)) AS Id
        , CAST(param2 AS char(50)) AS Name;

END$$

DELIMITER ;

