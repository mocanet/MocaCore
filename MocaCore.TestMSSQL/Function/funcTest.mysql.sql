﻿USE `MocaTest`;
DROP function IF EXISTS `funcTest`;

DELIMITER $$
USE `MocaTest`$$
CREATE FUNCTION `funcTest` ()
RETURNS INTEGER
BEGIN

RETURN 1;
END$$

DELIMITER ;

