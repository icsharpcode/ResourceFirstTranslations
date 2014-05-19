CREATE TABLE [dbo].[BranchResourceFiles] (
    [Id]                  INT            IDENTITY (1, 1) NOT NULL,
    [FK_BranchId]         INT            NOT NULL,
    [FK_ResourceFileId]   INT            NOT NULL,
    [SyncRawPathAbsolute] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_BranchResourceFiles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BranchResourceFiles_Branches] FOREIGN KEY ([FK_BranchId]) REFERENCES [dbo].[Branches] ([Id]),
    CONSTRAINT [FK_BranchResourceFiles_ResourceFiles] FOREIGN KEY ([FK_ResourceFileId]) REFERENCES [dbo].[ResourceFiles] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_BranchResourceFiles]
    ON [dbo].[BranchResourceFiles]([FK_BranchId] ASC, [FK_ResourceFileId] ASC);

