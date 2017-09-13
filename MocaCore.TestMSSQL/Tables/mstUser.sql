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

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'mstUser',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'名称',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'mstUser',
    @level2type = N'COLUMN',
    @level2name = N'Name'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'メール',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'mstUser',
    @level2type = N'COLUMN',
    @level2name = N'Mail'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'備考',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'mstUser',
    @level2type = N'COLUMN',
    @level2name = N'Note'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'管理者',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'mstUser',
    @level2type = N'COLUMN',
    @level2name = N'Admin'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'作成日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'mstUser',
    @level2type = N'COLUMN',
    @level2name = N'InsertDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'変更日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'mstUser',
    @level2type = N'COLUMN',
    @level2name = N'UpdateDate'