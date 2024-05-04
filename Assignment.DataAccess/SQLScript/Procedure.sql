CREATE PROCEDURE [dbo].[Sp_CreateSchemaTableCompanyWise]
(
	@schema VARCHAR(5)
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @sql NVARCHAR(MAX)
	--Check Schema Existing
	IF NOT EXISTS (SELECT TOP 1 1 FROM sys.schemas WHERE name = @schema)
	BEGIN
		--Create Schema
		EXEC('CREATE SCHEMA '+@schema);
	END
	
	--Create Table
	SET @sql=N'
		CREATE TABLE ['+@schema+'].[Product]
		(
			[Id] INT IDENTITY(1,1) PRIMARY KEY,
			[Title] VARCHAR(255) NOT NULL,
			[Description] NVARCHAR(255) NOT NULL,
			[Price] DECIMAL(18,2) DEFAULT(0),
			[CreatedBy] VARCHAR(255) NOT NULL,
			[CreatedDateTime] DATETIME NOT NULL,
			[UpdatedBy] VARCHAR(255) NULL,
			[UpdatedDateTime] DATETIME NULL
		)
	'
	EXECUTE(@sql)
	SET NOCOUNT OFF;
END

GO

CREATE PROCEDURE [dbo].[Sp_GetProductByFilter]
(
	@displayStart INT,
	@displayLength INT,
	@sortDir VARCHAR(10),
	@sortCol INT,
	@search VARCHAR(255),
	@schema VARCHAR(5)
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @sql NVARCHAR(MAX),@sqlStrAgg NVARCHAR(MAX), @errorMessage VARCHAR(4000)='', @errorLineNumber INT=0

	SET @sortCol=CASE WHEN @sortCol=0 THEN 1 ELSE @sortCol END

	SELECT @sqlStrAgg=STRING_AGG('SELECT Id,Title,[Description],CAST(Price AS NVARCHAR(20)) AS [Price],'''+CompanyName+''' AS [CompanyName],CreatedBy,FORMAT(CreatedDateTime,''dd-MM-yyyy HH:mm:ss'') AS [CreatedDateTime],ISNULL(UpdatedBy,'''') AS [UpdatedBy],ISNULL(FORMAT(UpdatedDateTime,''dd-MM-yyyy HH:mm:ss''),'''') AS [UpdatedDateTime],'+CAST(Id AS NVARCHAR)+' AS [CompanyInfoId] FROM ['+[Schema]+'].Product',' UNION ALL ') FROM CompanyInfos WHERE @schema='' OR [Schema]=@schema
	PRINT @sqlStrAgg

	SET @sql=N'
		SELECT
			ROW_NUMBER() OVER(ORDER BY A.Id) AS [RowNum],
			COUNT(1) OVER() AS [RecCount],
			A.*
		FROM 
		(
			'+@sqlStrAgg+'
		) AS A
		WHERE  ('''+@search+'''='''' OR A.Title LIKE '''+@search+'%'' OR A.[Description] LIKE '''+@search+'%'' OR A.[Price] LIKE '''+@search+'%'' OR A.[CompanyName] LIKE '''+@search+'%'' OR A.CreatedBy LIKE '''+@search+'%'' OR A.UpdatedBy LIKE '''+@search+'%'') ORDER BY '+CAST(@sortCol AS NVARCHAR(10))+' '+ @sortDir +' OFFSET '+ CAST(@displayStart AS NVARCHAR(10))+' ROW FETCH NEXT ' + CAST(@displayLength AS NVARCHAR(10))+' ROWS ONLY
	'
	PRINT(@sql)
	EXECUTE(@sql)
	SET NOCOUNT OFF;
END

GO

CREATE PROCEDURE [dbo].[Sp_GetCompanyInfoByFilter]
(
	@displayStart INT,
	@displayLength INT,
	@sortDir VARCHAR(10),
	@sortCol INT,
	@search VARCHAR(255)
)
AS
BEGIN
	DECLARE @sql NVARCHAR(MAX), @errorMessage VARCHAR(4000)='', @errorLineNumber INT=0

	SET @sortCol=CASE WHEN @sortCol=0 THEN 1 ELSE @sortCol END
	SELECT @sql=N'
		SELECT
			ROW_NUMBER() OVER(ORDER BY CI.Id) AS [RowNum],
			COUNT(1) OVER() AS [RecCount],
			CI.Id AS [Id],
			CI.CompanyName,
			CI.[Schema],
			CI.CreatedBy,
			FORMAT(CI.CreatedDateTime,''dd-MM-yyyy HH:mm:ss'') AS [CreatedDateTime],
			ISNULL(CI.UpdatedBy,'''') AS [UpdatedBy],
			ISNULL(FORMAT(CI.UpdatedDateTime,''dd-MM-yyyy HH:mm:ss''),'''') AS [UpdatedDateTime]
		FROM CompanyInfos AS CI
		WHERE  ('''+@search+'''='''' OR CI.CompanyName LIKE '''+@search+'%'' OR CI.[Schema] LIKE '''+@search+'%'' OR CI.CreatedBy LIKE '''+@search+'%'' OR CI.UpdatedBy LIKE '''+@search+'%'') ORDER BY '+CAST(@sortCol AS NVARCHAR(10))+' '+ @sortDir +' OFFSET '+ CAST(@displayStart AS NVARCHAR(10))+' ROW FETCH NEXT ' + CAST(@displayLength AS NVARCHAR(10))+' ROWS ONLY
	' 
	PRINT @sql
	EXECUTE(@sql)
END