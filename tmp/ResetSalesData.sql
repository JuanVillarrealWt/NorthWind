BEGIN TRANSACTION;
DELETE FROM [OrderDetails]; DELETE FROM [Orders];

DELETE FROM [Products]; DELETE FROM [Customers];

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'CurrentBalance') AND [object_id] = OBJECT_ID(N'[Customers]'))
    SET IDENTITY_INSERT [Customers] ON;
INSERT INTO [Customers] ([Id], [Name], [CurrentBalance])
VALUES (N'ALFKI', N'Alfreds Futterkiste', 0.0),
(N'ANATR', N'Ana Trujillo Emparedados y helados', 0.0),
(N'ANTON', N'Antonio Moreno Taquería', 100.0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'CurrentBalance') AND [object_id] = OBJECT_ID(N'[Customers]'))
    SET IDENTITY_INSERT [Customers] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'UnitPrice', N'UnitsInStock') AND [object_id] = OBJECT_ID(N'[Products]'))
    SET IDENTITY_INSERT [Products] ON;
INSERT INTO [Products] ([Id], [Name], [UnitPrice], [UnitsInStock])
VALUES (1, N'Chai', 35.0, CAST(20 AS smallint)),
(2, N'Chang', 55.0, CAST(0 AS smallint)),
(3, N'Aniseed Syrup', 65.0, CAST(20 AS smallint)),
(4, N'Chef Anton''s Cajun Seasoning', 75.0, CAST(40 AS smallint)),
(5, N'Chef Anton''s Gumbo Mix', 50.0, CAST(20 AS smallint));
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'UnitPrice', N'UnitsInStock') AND [object_id] = OBJECT_ID(N'[Products]'))
    SET IDENTITY_INSERT [Products] OFF;

DBCC CHECKIDENT ('[Products]', RESEED, 5);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261019201923_ResetSalesData', N'10.0.12');

COMMIT;
GO

