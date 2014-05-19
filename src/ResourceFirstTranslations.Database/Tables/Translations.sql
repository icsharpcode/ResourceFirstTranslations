CREATE TABLE [dbo].[Translations] (
    [Id]                                       INT            IDENTITY (1, 1) NOT NULL,
    [FK_BranchId]                              INT            NOT NULL,
    [FK_ResourceFileId]                        INT            NOT NULL,
    [ResourceIdentifier]                       NVARCHAR (255) NOT NULL,
    [Culture]                                  NVARCHAR (10)  NOT NULL,
    [OriginalResxValueAtTranslation]           NVARCHAR (MAX) NULL,
    [OriginalResxValueChangedSinceTranslation] BIT            CONSTRAINT [DF_Translations_OriginalChangedSinceTranslation] DEFAULT ((0)) NOT NULL,
    [TranslatedValue]                          NVARCHAR (MAX) NULL,
    [LastUpdated]                              DATETIME       NOT NULL,
    [LastUpdatedBy]                            NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_Translations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Translations_Branches] FOREIGN KEY ([FK_BranchId]) REFERENCES [dbo].[Branches] ([Id]),
    CONSTRAINT [FK_Translations_ResourceFiles] FOREIGN KEY ([FK_ResourceFileId]) REFERENCES [dbo].[ResourceFiles] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Translations]
    ON [dbo].[Translations]([FK_BranchId] ASC, [FK_ResourceFileId] ASC, [ResourceIdentifier] ASC, [Culture] ASC);

