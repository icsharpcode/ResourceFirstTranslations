CREATE TABLE [dbo].[Users] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [UserName]     NVARCHAR (50)  NOT NULL,
    [PasswordHash] NVARCHAR (255) NOT NULL,
    [PasswordSalt] NVARCHAR (255) NOT NULL,
    [EmailAddress] NVARCHAR (128) NOT NULL,
    [FirstName]    NVARCHAR (50)  NOT NULL,
    [LastName]     NVARCHAR (50)  NOT NULL,
    [IsActive]     BIT            CONSTRAINT [DF_Users_IsActive] DEFAULT ((1)) NOT NULL,
    [IsAdmin]      BIT            CONSTRAINT [DF_Users_IsAdmin] DEFAULT ((1)) NOT NULL,
    [TS]           ROWVERSION     NULL,
    [Cultures]     NVARCHAR (255) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserName_Identity]
    ON [dbo].[Users]([UserName] ASC);

