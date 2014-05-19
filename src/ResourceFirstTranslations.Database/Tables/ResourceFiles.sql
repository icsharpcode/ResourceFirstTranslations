CREATE TABLE [dbo].[ResourceFiles] (
    [Id]                      INT            NOT NULL,
    [ResourceFileDisplayName] NVARCHAR (50)  NOT NULL,
    [ResourceFileNameFormat]  NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_ResourceFiles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

