CREATE TABLE [dbo].[ResourceStrings] (
    [Id]                        INT            IDENTITY (1, 1) NOT NULL,
    [FK_BranchId]               INT            NOT NULL,
    [FK_ResourceFileId]         INT            NOT NULL,
    [ResourceIdentifier]        NVARCHAR (255) NOT NULL,
    [ResxValue]                 NVARCHAR (MAX) NULL,
    [ResxComment]               NVARCHAR (MAX) NULL,
    [LastUpdatedFromRepository] DATETIME       NOT NULL,
    CONSTRAINT [PK_ResourceStrings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ResourceStrings_Branches] FOREIGN KEY ([FK_BranchId]) REFERENCES [dbo].[Branches] ([Id]),
    CONSTRAINT [FK_ResourceStrings_ResourceFiles] FOREIGN KEY ([FK_ResourceFileId]) REFERENCES [dbo].[ResourceFiles] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ResourceStrings]
    ON [dbo].[ResourceStrings]([FK_BranchId] ASC, [FK_ResourceFileId] ASC, [ResourceIdentifier] ASC);

