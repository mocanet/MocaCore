﻿CREATE TABLE `MocaTest`.`mstUser` (
  `Id` VARCHAR(50) NOT NULL COMMENT 'ID',
  `Name` NVARCHAR(50) NOT NULL COMMENT '名称',
  `Mail` VARCHAR(128) NULL COMMENT 'メール',
  `Note` NVARCHAR(50) NULL COMMENT '備考',
  `Admin` BIT NULL DEFAULT 0 COMMENT '管理者',
  `InsertDate` DATETIME NULL COMMENT '作成日',
  `UpdateDate` DATETIME NULL COMMENT '変更日',
  PRIMARY KEY (`Id`))
COMMENT = 'ユーザー情報';
