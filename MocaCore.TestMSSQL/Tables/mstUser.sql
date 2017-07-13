CREATE TABLE [dbo].[mstUser]
(
	[Id] VARCHAR(50) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Mail] VARCHAR(128) NULL, 
    [Note] NVARCHAR(50) NULL, 
    [Admin] BIT NOT NULL DEFAULT 0,
    [InsertDate] DATETIME NOT NULL, 
    [UpdateDate] DATETIME NOT NULL 
)
