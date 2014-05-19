/****** Object:  StoredProcedure [dbo].[sp_getalltranslationsforresource]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP PROCEDURE [dbo].[sp_getalltranslationsforresource]
GO
ALTER TABLE [dbo].[Translations] DROP CONSTRAINT [FK_Translations_ResourceFiles]
GO
ALTER TABLE [dbo].[Translations] DROP CONSTRAINT [FK_Translations_Branches]
GO
ALTER TABLE [dbo].[ResourceStrings] DROP CONSTRAINT [FK_ResourceStrings_ResourceFiles]
GO
ALTER TABLE [dbo].[ResourceStrings] DROP CONSTRAINT [FK_ResourceStrings_Branches]
GO
ALTER TABLE [dbo].[BranchResourceFiles] DROP CONSTRAINT [FK_BranchResourceFiles_ResourceFiles]
GO
ALTER TABLE [dbo].[BranchResourceFiles] DROP CONSTRAINT [FK_BranchResourceFiles_Branches]
GO
/****** Object:  Index [IX_UserName_Identity]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP INDEX [IX_UserName_Identity] ON [dbo].[Users]
GO
/****** Object:  Index [IX_Translations]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP INDEX [IX_Translations] ON [dbo].[Translations]
GO
/****** Object:  Index [IX_ResourceStrings]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP INDEX [IX_ResourceStrings] ON [dbo].[ResourceStrings]
GO
/****** Object:  Index [IX_BranchResourceFiles]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP INDEX [IX_BranchResourceFiles] ON [dbo].[BranchResourceFiles]
GO
/****** Object:  View [dbo].[BranchResourceFilesView]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP VIEW [dbo].[BranchResourceFilesView]
GO
/****** Object:  View [dbo].[MissingTranslationsView]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP VIEW [dbo].[MissingTranslationsView]
GO
/****** Object:  View [dbo].[TranslationsAllBranchesView]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP VIEW [dbo].[TranslationsAllBranchesView]
GO
/****** Object:  View [dbo].[TranslationsJoinedResourceStringsView]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP VIEW [dbo].[TranslationsJoinedResourceStringsView]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP TABLE [dbo].[Users]
GO
/****** Object:  Table [dbo].[Translations]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP TABLE [dbo].[Translations]
GO
/****** Object:  Table [dbo].[ResourceStrings]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP TABLE [dbo].[ResourceStrings]
GO
/****** Object:  Table [dbo].[ResourceFiles]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP TABLE [dbo].[ResourceFiles]
GO
/****** Object:  Table [dbo].[Languages]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP TABLE [dbo].[Languages]
GO
/****** Object:  Table [dbo].[BranchResourceFiles]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP TABLE [dbo].[BranchResourceFiles]
GO
/****** Object:  Table [dbo].[Branches]    Script Date: 5/13/2014 4:11:43 PM ******/
DROP TABLE [dbo].[Branches]
GO

/****** Object:  Table [dbo].[Branches]    Script Date: 5/13/2014 4:07:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Branches](
	[Id] [int] NOT NULL,
	[BranchDisplayName] [nvarchar](50) NOT NULL,
	[BranchRootUrl] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Branches] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BranchResourceFiles]    Script Date: 5/13/2014 4:07:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BranchResourceFiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FK_BranchId] [int] NOT NULL,
	[FK_ResourceFileId] [int] NOT NULL,
	[SyncRawPathAbsolute] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_BranchResourceFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Languages]    Script Date: 5/13/2014 4:07:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Languages](
	[Culture] [nvarchar](10) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED 
(
	[Culture] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ResourceFiles]    Script Date: 5/13/2014 4:07:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceFiles](
	[Id] [int] NOT NULL,
	[ResourceFileDisplayName] [nvarchar](50) NOT NULL,
	[ResourceFileNameFormat] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_ResourceFiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ResourceStrings]    Script Date: 5/13/2014 4:07:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ResourceStrings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FK_BranchId] [int] NOT NULL,
	[FK_ResourceFileId] [int] NOT NULL,
	[ResourceIdentifier] [nvarchar](255) NOT NULL,
	[ResxValue] [nvarchar](max) NULL,
	[ResxComment] [nvarchar](max) NULL,
	[LastUpdatedFromRepository] [datetime] NOT NULL,
 CONSTRAINT [PK_ResourceStrings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Translations]    Script Date: 5/13/2014 4:07:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Translations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FK_BranchId] [int] NOT NULL,
	[FK_ResourceFileId] [int] NOT NULL,
	[ResourceIdentifier] [nvarchar](255) NOT NULL,
	[Culture] [nvarchar](10) NOT NULL,
	[OriginalResxValueAtTranslation] [nvarchar](max) NULL,
	[OriginalResxValueChangedSinceTranslation] [bit] NOT NULL CONSTRAINT [DF_Translations_OriginalChangedSinceTranslation]  DEFAULT ((0)),
	[TranslatedValue] [nvarchar](max) NULL,
	[LastUpdated] [datetime] NOT NULL,
	[LastUpdatedBy] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Translations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/13/2014 4:07:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[PasswordSalt] [nvarchar](255) NOT NULL,
	[EmailAddress] [nvarchar](128) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF_Users_IsActive]  DEFAULT ((1)),
	[IsAdmin] [bit] NOT NULL CONSTRAINT [DF_Users_IsAdmin]  DEFAULT ((1)),
	[TS] [timestamp] NULL,
	[Cultures] [nvarchar](255) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[TranslationsJoinedResourceStringsView]    Script Date: 5/13/2014 4:07:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[TranslationsJoinedResourceStringsView]
AS
SELECT        dbo.Translations.*,dbo.Branches.BranchDisplayName, dbo.ResourceStrings.ResxValue, dbo.ResourceStrings.ResxComment, dbo.ResourceStrings.LastUpdatedFromRepository
FROM            dbo.ResourceStrings INNER JOIN
                         dbo.Translations ON dbo.ResourceStrings.FK_BranchId = dbo.Translations.FK_BranchId AND dbo.ResourceStrings.FK_ResourceFileId = dbo.Translations.FK_ResourceFileId AND 
                         dbo.ResourceStrings.ResourceIdentifier = dbo.Translations.ResourceIdentifier
                                         INNER JOIN dbo.Branches on dbo.Translations.FK_BranchId = dbo.Branches.Id


GO
/****** Object:  View [dbo].[TranslationsAllBranchesView]    Script Date: 5/13/2014 4:07:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[TranslationsAllBranchesView]
AS
SELECT        *
FROM            (SELECT        *, ROW_NUMBER() OVER (PARTITION BY FK_ResourceFileId, ResourceIdentifier, Culture
                          ORDER BY FK_BranchId DESC) AS RowNumber
FROM            TranslationsJoinedResourceStringsView) MyProjection
WHERE        RowNumber = 1

GO
/****** Object:  View [dbo].[MissingTranslationsView]    Script Date: 5/13/2014 4:07:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[MissingTranslationsView]
AS
SELECT        Culture, FK_BranchId, COUNT(*) AS MissingTranslations
FROM            dbo.TranslationsJoinedResourceStringsView
WHERE        (OriginalResxValueChangedSinceTranslation = 1)
GROUP BY Culture, FK_BranchId

GO
/****** Object:  View [dbo].[BranchResourceFilesView]    Script Date: 5/13/2014 4:07:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[BranchResourceFilesView]
AS
SELECT        dbo.BranchResourceFiles.*, dbo.Branches.BranchDisplayName, dbo.Branches.BranchRootUrl, dbo.ResourceFiles.ResourceFileDisplayName, dbo.ResourceFiles.ResourceFileNameFormat
FROM            dbo.BranchResourceFiles INNER JOIN
                         dbo.Branches ON dbo.BranchResourceFiles.FK_BranchId = dbo.Branches.Id INNER JOIN
                         dbo.ResourceFiles ON dbo.BranchResourceFiles.FK_ResourceFileId = dbo.ResourceFiles.Id

GO
/****** Object:  Index [IX_BranchResourceFiles]    Script Date: 5/13/2014 4:07:54 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_BranchResourceFiles] ON [dbo].[BranchResourceFiles]
(
	[FK_BranchId] ASC,
	[FK_ResourceFileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ResourceStrings]    Script Date: 5/13/2014 4:07:54 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ResourceStrings] ON [dbo].[ResourceStrings]
(
	[FK_BranchId] ASC,
	[FK_ResourceFileId] ASC,
	[ResourceIdentifier] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Translations]    Script Date: 5/13/2014 4:07:54 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Translations] ON [dbo].[Translations]
(
	[FK_BranchId] ASC,
	[FK_ResourceFileId] ASC,
	[ResourceIdentifier] ASC,
	[Culture] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserName_Identity]    Script Date: 5/13/2014 4:07:54 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserName_Identity] ON [dbo].[Users]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BranchResourceFiles]  WITH CHECK ADD  CONSTRAINT [FK_BranchResourceFiles_Branches] FOREIGN KEY([FK_BranchId])
REFERENCES [dbo].[Branches] ([Id])
GO
ALTER TABLE [dbo].[BranchResourceFiles] CHECK CONSTRAINT [FK_BranchResourceFiles_Branches]
GO
ALTER TABLE [dbo].[BranchResourceFiles]  WITH CHECK ADD  CONSTRAINT [FK_BranchResourceFiles_ResourceFiles] FOREIGN KEY([FK_ResourceFileId])
REFERENCES [dbo].[ResourceFiles] ([Id])
GO
ALTER TABLE [dbo].[BranchResourceFiles] CHECK CONSTRAINT [FK_BranchResourceFiles_ResourceFiles]
GO
ALTER TABLE [dbo].[ResourceStrings]  WITH CHECK ADD  CONSTRAINT [FK_ResourceStrings_Branches] FOREIGN KEY([FK_BranchId])
REFERENCES [dbo].[Branches] ([Id])
GO
ALTER TABLE [dbo].[ResourceStrings] CHECK CONSTRAINT [FK_ResourceStrings_Branches]
GO
ALTER TABLE [dbo].[ResourceStrings]  WITH CHECK ADD  CONSTRAINT [FK_ResourceStrings_ResourceFiles] FOREIGN KEY([FK_ResourceFileId])
REFERENCES [dbo].[ResourceFiles] ([Id])
GO
ALTER TABLE [dbo].[ResourceStrings] CHECK CONSTRAINT [FK_ResourceStrings_ResourceFiles]
GO
ALTER TABLE [dbo].[Translations]  WITH CHECK ADD  CONSTRAINT [FK_Translations_Branches] FOREIGN KEY([FK_BranchId])
REFERENCES [dbo].[Branches] ([Id])
GO
ALTER TABLE [dbo].[Translations] CHECK CONSTRAINT [FK_Translations_Branches]
GO
ALTER TABLE [dbo].[Translations]  WITH CHECK ADD  CONSTRAINT [FK_Translations_ResourceFiles] FOREIGN KEY([FK_ResourceFileId])
REFERENCES [dbo].[ResourceFiles] ([Id])
GO
ALTER TABLE [dbo].[Translations] CHECK CONSTRAINT [FK_Translations_ResourceFiles]
GO
/****** Object:  StoredProcedure [dbo].[sp_getalltranslationsforresource]    Script Date: 5/13/2014 4:07:54 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_getalltranslationsforresource](@doc ntext)
AS
BEGIN
	DECLARE @xml xml
	SET @xml = CONVERT(xml, @doc)

	SELECT t.*        
	FROM dbo.TranslationsJoinedResourceStringsView t INNER JOIN
	( 
	SELECT
		 doc.col.value('@FileId', 'int') fileId
		,doc.col.value('@ResourceId', 'nvarchar(255)') resourceId 
		,doc.col.value('@Culture', 'nvarchar(10)') culture
	FROM @xml.nodes('/Translations/Item') doc(col)
	) x
	ON t.FK_ResourceFileId = x.FileId AND
	   t.ResourceIdentifier = x.ResourceId AND
	   t.Culture = x.Culture
END

GO
