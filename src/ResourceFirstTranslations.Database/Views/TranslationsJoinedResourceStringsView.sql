CREATE VIEW [dbo].[TranslationsJoinedResourceStringsView]
AS
SELECT        dbo.Translations.*,dbo.Branches.BranchDisplayName, dbo.ResourceStrings.ResxValue, dbo.ResourceStrings.ResxComment, dbo.ResourceStrings.LastUpdatedFromRepository
FROM            dbo.ResourceStrings INNER JOIN
                         dbo.Translations ON dbo.ResourceStrings.FK_BranchId = dbo.Translations.FK_BranchId AND dbo.ResourceStrings.FK_ResourceFileId = dbo.Translations.FK_ResourceFileId AND 
                         dbo.ResourceStrings.ResourceIdentifier = dbo.Translations.ResourceIdentifier
                                         INNER JOIN dbo.Branches on dbo.Translations.FK_BranchId = dbo.Branches.Id


GO
