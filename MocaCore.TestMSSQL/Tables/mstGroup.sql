CREATE TABLE [dbo].[mstGroup]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Note] NVARCHAR(50) NULL, 
    [InsertDate] DATETIME NOT NULL, 
    [UpdateDate] DATETIME NOT NULL
)
