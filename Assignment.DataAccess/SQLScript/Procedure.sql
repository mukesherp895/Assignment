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

CREATE PROCEDURE Sp_ProductGet
(
	@schema VARCHAR(5)
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @sql NVARCHAR(MAX)
	SELECT @sql=STRING_AGG('SELECT Id,Title,[Description],Price,'+CAST(Id AS NVARCHAR)+' AS [CompanyInfoId],'''+CompanyName+''' AS [CompanyName] FROM ['+[Schema]+'].Product',' UNION ALL ') FROM CompanyInfos WHERE @schema='' OR [Schema]=@schema
	PRINT @sql
	EXECUTE(@sql)
	SET NOCOUNT OFF;
END