BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[WorkItemStates]') AND [c].[name] = N'Value');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [WorkItemStates] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [WorkItemStates] ALTER COLUMN [Value] nvarchar(60) NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230621114657_StateMaxLength', N'6.0.18');
GO

COMMIT;
GO

