IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200620044615_Migration1')
BEGIN
    CREATE TABLE [ProductItems] (
        [ProductID] bigint NOT NULL IDENTITY,
        [Brand] nvarchar(50) NOT NULL,
        [Flavour] nvarchar(60) NOT NULL,
        [Alcoholic] bit NOT NULL,
        [ContainerType] nvarchar(10) NOT NULL,
        [Returnable] bit NOT NULL,
        [StockAmount] int NOT NULL,
        [SellPrice] decimal(18,2) NOT NULL,
        [Discount] int NOT NULL,
        CONSTRAINT [PK_ProductItems] PRIMARY KEY ([ProductID])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200620044615_Migration1')
BEGIN
    CREATE TABLE [OrderItems] (
        [OrderID] bigint NOT NULL IDENTITY,
        [TotalCost] decimal(18,2) NOT NULL,
        [QuantityOrdered] int NOT NULL,
        [OrderDate] datetime2 NOT NULL,
        [ProductID] bigint NULL,
        CONSTRAINT [PK_OrderItems] PRIMARY KEY ([OrderID]),
        CONSTRAINT [FK_OrderItems_ProductItems_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [ProductItems] ([ProductID]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200620044615_Migration1')
BEGIN
    CREATE TABLE [SellItems] (
        [SellID] bigint NOT NULL IDENTITY,
        [Quantity] int NOT NULL,
        [TotalCost] decimal(18,2) NOT NULL,
        [ContainerReturned] bit NOT NULL,
        [Payed] decimal(18,2) NOT NULL,
        [SellDate] datetime2 NOT NULL,
        [ProductID] bigint NULL,
        CONSTRAINT [PK_SellItems] PRIMARY KEY ([SellID]),
        CONSTRAINT [FK_SellItems_ProductItems_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [ProductItems] ([ProductID]) ON DELETE NO ACTION
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200620044615_Migration1')
BEGIN
    CREATE INDEX [IX_OrderItems_ProductID] ON [OrderItems] ([ProductID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200620044615_Migration1')
BEGIN
    CREATE INDEX [IX_SellItems_ProductID] ON [SellItems] ([ProductID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200620044615_Migration1')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20200620044615_Migration1', N'3.1.2');
END;

GO

