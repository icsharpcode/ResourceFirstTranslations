CREATE VIEW dbo.TranslationsAllBranchesView
AS
SELECT *
FROM (SELECT *, ROW_NUMBER() OVER 
        (PARTITION BY FK_ResourceFileId, ResourceIdentifier, Culture ORDER BY FK_BranchId DESC) AS RowNumber
FROM TranslationsJoinedResourceStringsView) MyProjection
WHERE RowNumber = 1

GO
