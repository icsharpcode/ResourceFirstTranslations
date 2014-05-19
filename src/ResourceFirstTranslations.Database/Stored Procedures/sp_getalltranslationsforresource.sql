
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
