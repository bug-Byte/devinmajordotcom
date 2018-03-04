/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

SELECT ROW_NUMBER() Over(Order by t.TABLE_NAME) as ID, t.TABLE_NAME AS TableName, t.TABLE_SCHEMA AS TableSchema
INTO #TempTables
FROM INFORMATION_SCHEMA.TABLES t WHERE t.TABLE_TYPE='BASE TABLE';

DECLARE @Counter INT = 1;
DECLARE @Date DATETIME;
SET @Date = GETDATE();
DECLARE @User VARCHAR(MAX) = 'Admin';

WHILE (@Counter <= (SELECT MAX(ID) FROM #TempTables))
BEGIN
	DECLARE @TableName VARCHAR(MAX) = (SELECT TableName FROM #TempTables WHERE ID = @Counter);
	DECLARE @SchemaName VARCHAR(MAX) = (SELECT TableSchema FROM #TempTables WHERE ID = @Counter);


	DECLARE @Sql1 VARCHAR(MAX) = 
	CONCAT(
		'ALTER TABLE [', @SchemaName, '].[', @TableName, ']', 
		' ADD CreatedOn DATETIME NULL, CreatedBy VARCHAR(MAX) NULL, ModifiedOn DATETIME NULL, ModifiedBy VARCHAR(MAX) NULL'
	);
	DECLARE @Sql2 VARCHAR(MAX) = 
	CONCAT
	(
		'UPDATE [', @SchemaName, '].[', @TableName, ']',
		' SET CreatedOn = ''', 
		@Date,
		''', ModifiedOn = ''', 
		@Date, 
		''', CreatedBy = ''', 
		@User, 
		''', ModifiedBy = ''', 
		@User,
		''';'
	)
	EXEC(@Sql1);
	SELECT @Sql2
	EXEC(@Sql2);

	SET @Counter = @Counter + 1;
END
GO

DROP TABLE #TempTables;
GO