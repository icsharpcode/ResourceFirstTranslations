-- Branches

INSERT INTO [dbo].[Branches] ([Id],[BranchDisplayName],[BranchRootUrl])
     VALUES
           (500,
           '5.x',
           'https://github.com/icsharpcode/SharpDevelop')
GO
INSERT INTO [dbo].[Branches] ([Id],[BranchDisplayName],[BranchRootUrl])
     VALUES
           (400,
           '4.x',
           'https://github.com/icsharpcode/SharpDevelop/tree/4.x')
GO

-- Files

INSERT INTO [dbo].[ResourceFiles] ([Id],[ResourceFileDisplayName],[ResourceFileNameFormat])
     VALUES
           (1,
           'StringResources',
		   'StringResource.{0}.resources')
GO

-- Branch Resource File

INSERT INTO [dbo].[BranchResourceFiles]
           ([FK_BranchId]
           ,[FK_ResourceFileId]
           ,[SyncRawPathAbsolute])
     VALUES
           (500
           ,1
           ,'https://githubrawsomething/blah/blups')
GO

-- Resource Strings

INSERT INTO [dbo].[ResourceStrings] ([FK_BranchId],[FK_ResourceFileId],[ResourceIdentifier],[ResxValue],[ResxComment],[LastUpdatedFromRepository])
     VALUES
           (400
           ,1
           ,'X.Y'
           ,'some value'
           ,''
           ,GETDATE())
GO

INSERT INTO [dbo].[ResourceStrings] ([FK_BranchId],[FK_ResourceFileId],[ResourceIdentifier],[ResxValue],[ResxComment],[LastUpdatedFromRepository])
     VALUES
           (500
           ,1
           ,'X.Y'
           ,'some value'
           ,''
           ,GETDATE())
GO

INSERT INTO [dbo].[ResourceStrings] ([FK_BranchId],[FK_ResourceFileId],[ResourceIdentifier],[ResxValue],[ResxComment],[LastUpdatedFromRepository])
     VALUES
           (400
           ,1
           ,'A.B'
           ,'some value'
           ,''
           ,GETDATE())
GO

INSERT INTO [dbo].[ResourceStrings] ([FK_BranchId],[FK_ResourceFileId],[ResourceIdentifier],[ResxValue],[ResxComment],[LastUpdatedFromRepository])
     VALUES
           (500
           ,1
           ,'A.B'
           ,'more values'
           ,''
           ,GETDATE())
GO

-- Translations

INSERT INTO [dbo].[Translations] ([FK_BranchId],[FK_ResourceFileId],[ResourceIdentifier],[Culture],[OriginalResxValueAtTranslation],[OriginalResxValueChangedSinceTranslation],[TranslatedValue],[LastUpdated],[LastUpdatedBy])
     VALUES
           (500
           ,1
           ,'X.Y'
           ,'de'
           ,'some value'
           ,0
           ,'ein Wert'
           ,GETDATE()
           ,'christophw')
GO

INSERT INTO [dbo].[Translations] ([FK_BranchId],[FK_ResourceFileId],[ResourceIdentifier],[Culture],[OriginalResxValueAtTranslation],[OriginalResxValueChangedSinceTranslation],[TranslatedValue],[LastUpdated],[LastUpdatedBy])
     VALUES
           (400
           ,1
           ,'X.Y'
           ,'de'
           ,'some value'
           ,0
           ,'ein Wert'
           ,GETDATE()
           ,'christophw')
GO

INSERT INTO [dbo].[Translations] ([FK_BranchId],[FK_ResourceFileId],[ResourceIdentifier],[Culture],[OriginalResxValueAtTranslation],[OriginalResxValueChangedSinceTranslation],[TranslatedValue],[LastUpdated],[LastUpdatedBy])
     VALUES
           (500
           ,1
           ,'A.B'
           ,'de'
           ,'more values'
           ,0
           ,'weitere Werte'
           ,GETDATE()
           ,'christophw')
GO

INSERT INTO [dbo].[Translations] ([FK_BranchId],[FK_ResourceFileId],[ResourceIdentifier],[Culture],[OriginalResxValueAtTranslation],[OriginalResxValueChangedSinceTranslation],[TranslatedValue],[LastUpdated],[LastUpdatedBy])
     VALUES
           (400
           ,1
           ,'A.B'
           ,'de'
           ,'some value'
           ,0
           ,'weitere Werte'
           ,GETDATE()
           ,'christophw')
GO

