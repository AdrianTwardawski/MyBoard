IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Tags] (
    [Id] int NOT NULL IDENTITY,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_Tags] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] uniqueidentifier NOT NULL,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [WorkItemStates] (
    [Id] int NOT NULL IDENTITY,
    [Value] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_WorkItemStates] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Addresses] (
    [Id] uniqueidentifier NOT NULL,
    [Country] nvarchar(max) NULL,
    [City] nvarchar(max) NULL,
    [Street] nvarchar(max) NULL,
    [PostalCode] nvarchar(max) NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Addresses_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [WorkItems] (
    [Id] int NOT NULL IDENTITY,
    [StateId] int NOT NULL,
    [Area] varchar(200) NULL,
    [Iteration_Path] nvarchar(max) NULL,
    [Priority] int NOT NULL DEFAULT 1,
    [AuthorId] uniqueidentifier NOT NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [StartDate] datetime2 NULL,
    [EndDate] datetime2(3) NULL,
    [Efford] decimal(5,2) NULL,
    [Activity] nvarchar(200) NULL,
    [RemainingWork] decimal(14,2) NULL,
    CONSTRAINT [PK_WorkItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_WorkItems_Users_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_WorkItems_WorkItemStates_StateId] FOREIGN KEY ([StateId]) REFERENCES [WorkItemStates] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Comments] (
    [Id] int NOT NULL IDENTITY,
    [Message] nvarchar(max) NULL,
    [Author] nvarchar(max) NULL,
    [CreatedDate] datetime2 NOT NULL DEFAULT (getutcdate()),
    [UpdatedDate] datetime2 NULL,
    [WorkItemId] int NOT NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Comments_WorkItems_WorkItemId] FOREIGN KEY ([WorkItemId]) REFERENCES [WorkItems] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [WorkItemTag] (
    [WorkItemId] int NOT NULL,
    [TagId] int NOT NULL,
    [PublicationDate] datetime2 NOT NULL DEFAULT (getutcdate()),
    CONSTRAINT [PK_WorkItemTag] PRIMARY KEY ([TagId], [WorkItemId]),
    CONSTRAINT [FK_WorkItemTag_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [Tags] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_WorkItemTag_WorkItems_WorkItemId] FOREIGN KEY ([WorkItemId]) REFERENCES [WorkItems] ([Id]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [IX_Addresses_UserId] ON [Addresses] ([UserId]);
GO

CREATE INDEX [IX_Comments_WorkItemId] ON [Comments] ([WorkItemId]);
GO

CREATE INDEX [IX_WorkItems_AuthorId] ON [WorkItems] ([AuthorId]);
GO

CREATE INDEX [IX_WorkItems_StateId] ON [WorkItems] ([StateId]);
GO

CREATE INDEX [IX_WorkItemTag_WorkItemId] ON [WorkItemTag] ([WorkItemId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230621113716_Init', N'6.0.18');
GO

COMMIT;
GO

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

BEGIN TRANSACTION;
GO

ALTER TABLE [Users] ADD [FullName] nvarchar(max) NULL;
GO


                UPDATE Users
                SET FullName = FirstName + ' ' + LastName
            
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'FirstName');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Users] DROP COLUMN [FirstName];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'LastName');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Users] DROP COLUMN [LastName];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230621122157_UserFullName', N'6.0.18');
GO

COMMIT;
GO

