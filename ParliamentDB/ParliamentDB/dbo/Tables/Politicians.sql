CREATE TABLE [dbo].[Politicians] (
    [Id]   INT           NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    [Post] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Politicians] PRIMARY KEY CLUSTERED ([Id] ASC)
);

