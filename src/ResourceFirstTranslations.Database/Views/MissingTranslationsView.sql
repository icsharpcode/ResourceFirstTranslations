CREATE VIEW dbo.MissingTranslationsView
AS
SELECT        Culture, FK_BranchId, COUNT(*) AS MissingTranslations
FROM            dbo.TranslationsJoinedResourceStringsView
WHERE        (OriginalResxValueChangedSinceTranslation = 1)
GROUP BY Culture, FK_BranchId

GO
