CREATE VIEW dbo.BranchResourceFilesView
AS
SELECT        dbo.BranchResourceFiles.*, dbo.Branches.BranchDisplayName, dbo.Branches.BranchRootUrl, dbo.ResourceFiles.ResourceFileDisplayName, dbo.ResourceFiles.ResourceFileNameFormat
FROM            dbo.BranchResourceFiles INNER JOIN
                         dbo.Branches ON dbo.BranchResourceFiles.FK_BranchId = dbo.Branches.Id INNER JOIN
                         dbo.ResourceFiles ON dbo.BranchResourceFiles.FK_ResourceFileId = dbo.ResourceFiles.Id

GO
