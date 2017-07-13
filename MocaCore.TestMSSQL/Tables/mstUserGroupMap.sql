CREATE TABLE [dbo].[mstUserGroupMap]
(
	[UserId] VARCHAR(50) NOT NULL , 
    [GroupId] INT NOT NULL, 
    PRIMARY KEY ([UserId], [GroupId])
)
