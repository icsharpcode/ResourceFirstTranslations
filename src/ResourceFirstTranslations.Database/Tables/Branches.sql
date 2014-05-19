CREATE TABLE [dbo].[Branches] (
    [Id]                INT            NOT NULL,
    [BranchDisplayName] NVARCHAR (50)  NOT NULL,
    [BranchRootUrl]     NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Branches] PRIMARY KEY CLUSTERED ([Id] ASC)
);

