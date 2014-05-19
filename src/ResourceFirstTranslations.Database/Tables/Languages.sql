CREATE TABLE [dbo].[Languages] (
    [Culture]     NVARCHAR (10)  NOT NULL,
    [Description] NVARCHAR (255) NOT NULL,
    CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED ([Culture] ASC)
);

